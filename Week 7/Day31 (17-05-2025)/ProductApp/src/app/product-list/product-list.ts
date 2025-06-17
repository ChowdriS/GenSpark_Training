import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, HostListener, OnInit, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  debounceTime,
  distinctUntilChanged,
  Subject,
  switchMap,
  catchError,
  of,
  startWith,
} from 'rxjs';
import { ProductService } from '../services/productService';
import { Product } from '../models/productModel';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-list.html',
  styleUrls: ['./product-list.css'],
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private searchSubject = new Subject<string>();
  private cdr = inject(ChangeDetectorRef);

  products: Product[] = [];
  loading = false;
  error: string | null = null;
  skip = 0;
  limit = 30;
  total = 0;
  searchQuery = '';

  ngOnInit() {
    this.searchSubject.pipe(
      startWith(''),
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((query) => {
        this.loading = true;
        this.error = null;
        this.searchQuery = query;
        this.skip = 0; 
        
        return this.productService.searchProducts(query, this.skip, this.limit).pipe(
          catchError((err) => {
            this.error = 'Failed to load products';
            this.loading = false;
            return of({ products: [], total: 0, skip: 0, limit: 0 }); 
          })
        );
      })
    ).subscribe(response => {
      this.products = response.products;
      this.total = response.total;
      this.skip = response.products.length; 
      this.loading = false;
      this.cdr.detectChanges();
    });
  }

  @HostListener('window:scroll')
  onScroll() {
    if (this.loading || this.products.length >= this.total) return;

    const threshold = 100;
    const position = window.scrollY + window.innerHeight;
    const height = document.documentElement.scrollHeight;

    if (position > height - threshold) {
      this.loadMore();
    }
  }

  searchProducts(event: Event) {
    const query = (event.target as HTMLInputElement).value;
    this.searchSubject.next(query);
  }

  private loadMore() {
    if (this.loading) return;
    
    this.loading = true;
    this.productService.searchProducts(this.searchQuery, this.skip, this.limit)
    .subscribe(response => {
      this.products = [...this.products, ...response.products];
      this.total = response.total;
      this.skip += response.products.length;
      this.loading = false;
      this.cdr.detectChanges();
    });
  }
}