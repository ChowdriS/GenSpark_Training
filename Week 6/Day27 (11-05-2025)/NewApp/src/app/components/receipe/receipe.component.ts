import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-receipe',
  imports: [],
  templateUrl: './receipe.component.html',
  styleUrl: './receipe.component.css'
})
export class ReceipeComponent {
  @Input() recipe: any;
}
