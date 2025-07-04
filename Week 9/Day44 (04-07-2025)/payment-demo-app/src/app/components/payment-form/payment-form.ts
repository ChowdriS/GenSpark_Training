import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-payment-form',
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './payment-form.html',
  styleUrl: './payment-form.css',
  standalone : true
})
export class PaymentForm {
  fb = inject(FormBuilder);
  paymentStatus: string | null = null;

  form = this.fb.group({
    amount: [null as number | null, [Validators.required, Validators.min(1)]],
    customerName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    contact: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
  });

  payNow() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formValue = this.form.value;
    const options = {
      key: '', 
      amount: formValue.amount! * 100,
      currency: 'INR',
      name: formValue.customerName,
      // order_id : "order_QosqAw4uh9HHhv",
      description: 'Test Transaction',
      prefill: {
        name: formValue.customerName,
        email: formValue.email,
        contact: formValue.contact
      },
      handler: function (response: any) {
        alert('Payment successful: ' + response.razorpay_payment_id);
      },
      modal: {
        ondismiss: function () {
          alert('Payment Exited!');
        }
      }
    };

    const rzp = new (window as any).Razorpay(options);
    rzp.open();
  }
}
