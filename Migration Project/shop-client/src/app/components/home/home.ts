import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { CategoryList } from "../category-list/category-list";

@Component({
  selector: 'app-home',
  imports: [RouterOutlet, CategoryList,RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  currentYear = new Date().getFullYear();
}
