import { Component, signal, OnInit } from '@angular/core';
import { Order } from '../../model/Models';
import { OrderService } from '../../services/order-service';
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';
import { CommonModule, DatePipe } from '@angular/common';


@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.html',
  styleUrl: './order-list.css',
  imports: [DatePipe,CommonModule],
  standalone: true
})
export class OrderList implements OnInit {
  orders = signal<Order[]>([]);

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.orderService.getOrders().subscribe({
      next: (res: any) => {
        this.orders.set(res.$values);
      },
      error: (err:any) => console.error('Failed to load orders', err)
    });
  }

  export(): void {
    const ordersData = this.orders();

    if (!ordersData || ordersData.length === 0) {
      alert('No orders to export!');
      return;
    }

    const exportData = ordersData.map(order => ({
      OrderID: order.orderID,
      OrderName: order.orderName,
      OrderDate: order.orderDate ? new Date(order.orderDate).toLocaleString() : '',
      CustomerName: order.customerName ?? '',
      CustomerEmail: order.customerEmail ?? '',
      CustomerPhone: order.customerPhone ?? '',
      CustomerAddress: order.customerAddress ?? '',
      Status: order.status ?? '',
      PaymentType: order.paymentType ?? ''
    }));

    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);

    const workbook: XLSX.WorkBook = {
      Sheets: { 'Orders': worksheet },
      SheetNames: ['Orders']
    };

    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });

    this.saveAsExcelFile(excelBuffer, 'Orders_Export');
  }

  private saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
    saveAs(data, `${fileName}_${new Date().toISOString().slice(0,10)}.xlsx`);
  }
}
