import { Component, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProductService } from '../../services/product-service';
import { Product } from '../../model/Models';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart-service';

@Component({
  selector: 'app-product-detail',
  imports: [RouterLink,CommonModule],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.css'
})
export class ProductDetail {
  product = signal<Product | null>(null);

  constructor(
    private route: ActivatedRoute,
    private cartService: CartService,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.productService.getProductById(id).subscribe({
      next: (res:any) => {this.product.set(res);console.log(this.product())},
      error: (err:any) => console.error('Error loading product', err)
    });
  }
  addToCart(productId: number) {
    this.cartService.addToCart(productId).subscribe({
      next: (response: any) => {
        alert('Product added to cart successfully!');
      },
      error: (err:any) => {
        alert('Failed to add product to cart: ' + (err.message || 'Unknown error'));
      }
    });
  }
}
