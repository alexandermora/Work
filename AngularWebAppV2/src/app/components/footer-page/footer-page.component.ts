import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer-page',
  templateUrl: './footer-page.component.html',
  styleUrls: ['./footer-page.component.css']
})
export class FooterPageComponent implements OnInit
{
    year: any;
    constructor() { }

    ngOnInit()
    {
        const now = new Date();
        this.year = now.getFullYear();
    }

}
