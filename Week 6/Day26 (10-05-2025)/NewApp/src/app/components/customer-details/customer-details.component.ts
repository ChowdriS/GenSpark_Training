// customer-details.component.ts
import { NgFor } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-customer-details',
  imports: [FormsModule,NgFor],
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.css']
})
export class CustomerDetailsComponent {
  customers: any[] = [];

  addCustomer(form: any) {
    if (form.valid) {
      this.customers.push({
        name: form.value.name,
        age: form.value.age,
        likes: 0,
        dislikes: 0
      });
      form.reset();
    }
  }

  addLike(index: number) {
    this.customers[index].likes++;
  }

  addDislike(index: number) {
    this.customers[index].dislikes++;
  }
}