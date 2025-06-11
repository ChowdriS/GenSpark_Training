import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-new-component',
  imports: [FormsModule],
  templateUrl: './new-component.component.html',
  styleUrl: './new-component.component.css'
})
export class NewComponentComponent {
    name:string;
    like: boolean = false;
    className: string = "bi bi-arrow-through-heart";
    constructor(){
      this.name = "Ramu"
    }
    onButtonClick(uname:string){
      this.name = uname;
    }
    toggle(){
      // this.like = !this.like;
      console.log(this.className)
      if(this.className === "bi bi-arrow-through-heart"){
        this.className = "bi bi-arrow-through-heart-fill"
      }else{
        this.className = "bi bi-arrow-through-heart"
      }
    }
}
