import { Component, AfterViewInit, input, Input } from '@angular/core';
import { OrderService } from '../../services/order-service';
import { CartService } from '../../services/cart-service';

declare var paypal: any;
@Component({
  selector: 'app-paypall-button',
  imports: [],
  templateUrl: './paypall-button.html',
  styleUrl: './paypall-button.css'
})
export class PaypallButton implements AfterViewInit{
  constructor(private cartService :CartService) {}
  @Input() cash: number = 0;
ngAfterViewInit(): void {
    paypal.Buttons({
      style: {
        layout: 'vertical',
        color: 'gold',
        shape: 'pill',
        label: 'paypal'
      },
      createOrder: (data: any, actions: any) => {
        return actions.order.create({
          purchase_units: [{
            amount: {
              value: this.cash.toString()
            }
          }]
        });
      },
      onApprove: (data: any, actions: any) => {
        return actions.order.capture().then((details: any) => {
          alert('Transaction completed by ' + details.payer.name.given_name);
          console.log('Order details', details);
          const orderData = {
            CustomerName: details.payer.name.given_name,
            CustomerEmail: details.payer.email_address,
            CustomerPhone: details.payer.phone?.phone_number?.national_number ?? '9999999999',
            CustomerAddress: details.payer.address?.address_line_1 ?? '223,abc,cde',
            PaymentType: 'PayPal',
          };
          this.cartService.processOrder(orderData).subscribe({
            next: () => {
              alert('Order submitted successfully!');
              this.cartService.clearCart().subscribe({
                next: () => {
                  console.log('Cart cleared after order submission');
                }
              });
              window.location.href = '/home';
            },
            error: (err:any) => {
              console.error('Error processing order', err);
              alert('Failed to submit order. Please try again.');
            }
          });
        });
      },
      onError: (err: any) => {
        console.error(' PayPal Checkout error', err);
      }
    }).render('#paypal-button-container');
  }
}