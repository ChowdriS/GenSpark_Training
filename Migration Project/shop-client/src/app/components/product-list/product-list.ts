import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Product } from '../../model/Models';
import { ProductService } from '../../services/product-service';
import { CartService } from '../../services/cart-service';

@Component({
  selector: 'app-product-list',
  imports: [RouterLink,CommonModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css'
})
export class ProductList implements OnInit {
  products = signal<Product[]>([]);
    totalPages= signal<number>(1);
    currentPage= signal<number>(1);
  constructor(private productService: ProductService,private cartService: CartService) {}

  ngOnInit() {
    this.getProducts();
  }
  goToPage(page:number){
    this.currentPage.set(page);
    this.getProducts();
  }
  getProducts() {
    this.productService.getProductsWithOutCategory(1).subscribe({
      next: (response: any) => {
        this.products.set(response?.items?.$values);
        this.currentPage.set(response.page)
      },
      error: (err:any) => {
        console.error('Error fetching products', err);
      }
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
