import { Component, OnInit, OnDestroy, HostListener, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { Subject, BehaviorSubject, Observable, of, merge } from 'rxjs';
import { debounceTime, switchMap, tap, catchError, startWith, scan, map, takeUntil } from 'rxjs/operators';
import { ProductService } from '../../Service/ProductService';
import { Product } from '../../Model/Product';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class Home implements OnInit {
  private destroy$ = new Subject<void>();
  private productService = inject(ProductService);

  searchControl = new FormControl('');
  private loadMore$ = new Subject<void>();
  private isLoading$ = new BehaviorSubject<boolean>(false);

  private currentQuery = '';
  private currentSkip = 0;
  private hasMore = true;

  products$!: Observable<Product[]>;
  loading$ = this.isLoading$.asObservable();

  ngOnInit() {
    const searchTrigger$ = this.searchControl.valueChanges.pipe(
      startWith(''),
      debounceTime(1000),
      tap(() => {
        this.currentSkip = 0;
        this.hasMore = true;
      }),
      switchMap(query => this.loadProducts(query || '', 0, true))
    );

    const loadMoreTrigger$ = this.loadMore$.pipe(
      switchMap(() => this.loadProducts(this.currentQuery, this.currentSkip, false))
    );

    this.products$ = merge(searchTrigger$, loadMoreTrigger$).pipe(
      startWith({ products: [], reset: true } as { products: Product[]; reset: boolean }),
      scan((acc: Product[], curr: { products: Product[]; reset: boolean }) =>
        curr.reset ? curr.products : [...acc, ...curr.products],
        []
      ),
      tap(() => this.isLoading$.next(false)),
      takeUntil(this.destroy$)
    );
  }

  private loadProducts(query: string, skip: number, reset: boolean) {
    this.currentQuery = query;
    this.isLoading$.next(true);

    return this.productService.searchProducts(query, 10, skip).pipe(
      tap(res => {
        this.currentSkip = skip + res.products.length;
        this.hasMore = res.products.length === 10;
      }),
      map(res => ({ products: res.products, reset }))
    );
  }

  @HostListener('window:scroll')
  onScroll() {
    const pos = window.pageYOffset + window.innerHeight;
    const max = document.documentElement.scrollHeight;
    if (pos > max - 200 && this.hasMore && !this.isLoading$.value) {
      this.loadMore$.next();
    }
  }
}
