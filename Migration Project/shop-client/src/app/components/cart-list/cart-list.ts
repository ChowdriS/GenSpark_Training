import { Cart } from '../../model/Models';
import { Router, RouterLink } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CartService } from '../../services/cart-service';
import { PaypallButton } from "../paypall-button/paypall-button";

@Component({
  selector: 'app-cart-list',
  imports: [RouterLink, CommonModule, ReactiveFormsModule, PaypallButton],
  templateUrl: './cart-list.html',
  styleUrl: './cart-list.css'
})
export class CartList implements OnInit {
  cart: Cart[] = [];
  cartForm!: FormGroup;

  constructor(private fb: FormBuilder, private cartService :CartService ,private router : Router) {}

  ngOnInit(): void {
    // const storedCart = localStorage.getItem('cart');
    // this.cart = storedCart ? JSON.parse(storedCart) : [];
    this.getCart();
  }

  getCart() {
    this.cartService.getCart().subscribe({
      next:(res:any)=>{
        this.cart = res.$values as Cart[];
        this.buildForm();
      },
      error:(err:any)=>{
        console.error('Error fetching cart:', err);
      }
    })
  }

  buildForm(): void {
    const group: { [key: string]: FormControl } = {};

    for (const item of this.cart) {
      const controlName = 'quantity-' + item.product?.productId;
      group[controlName] = new FormControl(item.quantity, [
        Validators.required,
        Validators.min(1),
      ]);
    }

    this.cartForm = this.fb.group(group);
  }

  getTotal(): number {
    return this.cart.reduce((sum, item) => sum + (item.product as any) ?.price * item.quantity, 0);
  }

  updateCart(): void {
    const updatedQuantities: { [productId: number]: number } = {};

    for (const item of this.cart) {
      const controlName = 'quantity-' + (item.product as any).productId;
      const control = this.cartForm.get(controlName);
      if (control && control.valid) {
        const newQty = +control.value;
        if (newQty !== item.quantity) {
          updatedQuantities[(item.product as any).productId] = newQty;
        }
      }
    }

    if (Object.keys(updatedQuantities).length === 0) {
      alert('No changes detected in cart quantities.');
      return;
    }

    this.cartService.updateCart(updatedQuantities).subscribe({
      next: (res:any) => {
        alert('Cart updated successfully!');
        this.getCart();
      },
      error: (err:any) => {
        console.error('Error updating cart:', err);
        alert('Failed to update cart.');
      }
    });
  }


  removeFromCart(productId: number): void {
    this.cartService.deleteCart(productId).subscribe({
      next: (res:any) => {
        alert('Cart item deleted successfully!');
        this.getCart();
      },
      error: (err:any) => {
        console.error('Error deleting item in cart:', err);
        alert('Failed to delete item in cart.');
      }
    });
  }
  clearCart(): void {
    if(confirm('Are you sure you want to clear the cart? This action cannot be undone.')) {
        this.cartService.clearCart().subscribe({
        next: (res:any) => {
          alert('Cart cleared successfully!');
          this.getCart();
        },
        error: (err:any) => {
          console.error('Error clearing cart:', err);
          alert('Failed to clear cart.');
        }
      });
    }
    
  }
  checkout(): void {
    if(!confirm('Do you want to proceed to checkout?'))
      return; 
    else
    this.router.navigate(['/home/cart/checkout']);
  }
}