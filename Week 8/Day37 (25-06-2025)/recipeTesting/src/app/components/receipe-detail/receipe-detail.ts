import { Component, Input, OnInit, signal } from '@angular/core';

@Component({
  selector: 'app-receipe-detail',
  imports: [],
  templateUrl: './receipe-detail.html',
  styleUrl: './receipe-detail.css'
})
export class ReceipeDetail implements OnInit {
  @Input() recipe!:any;
  ngOnInit(): void {
    // console.log(this.recipe);
  }
}
