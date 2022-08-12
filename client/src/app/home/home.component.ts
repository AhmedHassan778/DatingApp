import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;

  constructor(private cdref:ChangeDetectorRef) { 
  }

  ngOnInit(): void {

  }

  registerModeToggle() {
    this.registerMode = !this.registerMode;
    console.log(this.registerMode);
    console.log("'>>.........");
    //this.cdref.detectChanges();
  }
  
  cancelRegisterMode(event: boolean){
    // debugger;
    this.registerMode=event;
  }
}
