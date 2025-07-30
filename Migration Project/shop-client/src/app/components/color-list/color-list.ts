import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Color } from '../../model/Models';
import { ColorService } from '../../services/color-service';

@Component({
  selector: 'app-color-list',
  imports: [RouterLink],
  templateUrl: './color-list.html',
  styleUrl: './color-list.css'
})
export class ColorList implements OnInit {
  Colors = signal<Color[]>([]);
  constructor(private colorService : ColorService) {
  }
  ngOnInit(): void {
    this.getColors();
  }
  getColors(){
    this.colorService.getColors().subscribe({
      next:(res:any)=>{
        this.Colors.set(res.$values);
      },
      error:(err:any)=>{
        console.log(err)
      }
    })
  }
  deleteColor(id:number){
    if(confirm("Are you sure, you want to delete?")){
      this.colorService.deleteColor(id).subscribe({
        next:(res:any)=>{
          alert("successfuly deleted!");
          this.getColors();
        },
        error:()=>{
          alert("something went wrong!");
        }
    })
    }
  }
}
