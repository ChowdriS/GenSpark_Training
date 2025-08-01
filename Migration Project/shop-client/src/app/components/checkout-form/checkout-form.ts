import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CartService } from '../../services/cart-service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-checkout-form',
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './checkout-form.html',
  styleUrl: './checkout-form.css'
})
export class CheckoutForm {
orderForm!: FormGroup;

  constructor(private fb: FormBuilder,private router : Router,private cartService: CartService) {}

  ngOnInit() {
    this.orderForm = this.fb.group({
      customerName: ['', Validators.required],
      customerPhone: ['', Validators.required],
      customerEmail: ['', [Validators.required, Validators.email]],
      customerAddress: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.orderForm.invalid) {
      this.orderForm.markAllAsTouched();
      return;
    }

    if (!confirm('Do you want to submit the order?')) {
      return; 
    }

    const orderData = {
      CustomerName: this.orderForm.value.customerName,
      CustomerPhone: this.orderForm.value.customerPhone,
      CustomerEmail: this.orderForm.value.customerEmail,
      CustomerAddress: this.orderForm.value.customerAddress,
      PaymentType: 'Cash on Delivery',
    };

    this.cartService.processOrder(orderData).subscribe({
      next: () => {
        alert('Order submitted successfully!');
        this.router.navigate(['/home']);
      },
      error: (err:any) => {
        console.error(err);
      }
    });
  }
}
