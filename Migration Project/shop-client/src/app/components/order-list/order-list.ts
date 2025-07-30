import { Component, signal } from '@angular/core';
import { Order } from '../../model/Models';
import { OrderService } from '../../services/order-service';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-order-list',
  imports: [RouterLink,DatePipe],
  templateUrl: './order-list.html',
  styleUrl: './order-list.css'
})
export class OrderList {
  orders = signal<Order[]>([]);
  // currentPage = 1;
  // totalPages = 1;

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.orderService.getOrders().subscribe({
      next: (res: any) => {
        this.orders.set(res.$values);
        // this.totalPages = res.totalPages;
        // this.currentPage = res.currentPage;
      },
      error: (err:any) => console.error('Failed to load orders', err)
    });
  }

  // goToPage(page: number): void {
  //   if (page >= 1 && page <= this.totalPages) {
  //     this.currentPage = page;
  //     this.loadOrders();
  //   }
  // }
}
