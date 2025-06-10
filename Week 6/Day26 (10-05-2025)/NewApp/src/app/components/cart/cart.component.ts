import { Component, Input } from '@angular/core';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-cart',
  imports: [NgFor],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
  @Input() cart: { [key: string]: { name: string, count: number } } = {};

  get total(): number {
    return Object.values(this.cart).reduce((sum, item) => sum + item.count, 0);
  }

  get itemList(): { name: string; count: number }[] {
    return Object.values(this.cart);
  }
}
