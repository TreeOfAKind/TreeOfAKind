import { Component, OnInit } from '@angular/core';
import { HomeService } from './home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  pingContent: string;

  constructor(
    private service: HomeService
  ) { }

  ngOnInit(): void {
  }

  pingClick() {
    this.service.ping(this.pingContent).subscribe();
  }

}
