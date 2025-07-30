import { Component, OnInit, signal } from '@angular/core';
import { Product } from '../../model/Models';
import { ProductService } from '../../services/product-service';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CartService } from '../../services/cart-service';

@Component({
  selector: 'app-category-products',
  imports: [CommonModule,RouterLink],
  templateUrl: './category-products.html',
  styleUrl: './category-products.css'
})
export class CategoryProducts implements OnInit {
  id! :string | null ;
products = signal<Product[]>([]);
    totalPages= signal<number>(1);
    currentPage= signal<number>(1);
  constructor(private productService: ProductService,private route : ActivatedRoute,private cartService : CartService) {}

  ngOnInit() {
    this.route.paramMap.subscribe(paramMap => {
      this.id = paramMap.get('id');
      if (this.id) {
        this.getProducts(this.id);
        this.getProducts(this.id);
      }
    });
  }
  goToPage(page:number){
    this.currentPage.set(page);
    this.getProducts(this.id);
  }
  getProducts(id: any) {
    this.productService.getProductsWithCategory(1,id).subscribe({
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