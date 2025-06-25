import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ReceipeList } from "./components/receipe-list/receipe-list";

@Component({
  selector: 'app-root',
  imports: [ReceipeList],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'recipeTesting';
}
