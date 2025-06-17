import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProductService } from '../services/productService';
import { Product } from '../models/productModel';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-detail.html',
  styleUrls: ['./product-detail.css'],
})
export class ProductDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private productService = inject(ProductService);
  private cdr = inject(ChangeDetectorRef);

  product: Product | null = null;
  loading = false;
  error: string | null = null;

  ngOnInit() {
    this.route.params.subscribe((params) => {
      const id = params['id'];
      if (id) this.loadProduct(id);
    });
  }

  loadProduct(id: string) {
    this.loading = true;
    this.error = null;
    this.product = null;

    this.productService.getProduct(id).subscribe({
      next: (product) => {
        this.product = { ...product };
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Product not found';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
