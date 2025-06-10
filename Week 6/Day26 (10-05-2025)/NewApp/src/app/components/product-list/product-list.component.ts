import { Component, EventEmitter, Output } from '@angular/core';
import { NgFor } from '@angular/common';
@Component({
  selector: 'app-product-list',
  imports: [NgFor],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent {
  @Output() cartUpdated = new EventEmitter<any>();
  products = [
    { name: 'Product 1', image: '../../../../assets/image.png' },
    { name: 'Product 2', image: '../../../../assets/image.png' },
    { name: 'Product 3', image: '../../../../assets/image.png' }
  ];

  addCart(product: any) {
    this.cartUpdated.emit({ name: product.name });
  }
}
