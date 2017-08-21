import { Component, OnInit } from '@angular/core';
import { GlobalVarService } from '../../_services/global-var.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit
{
    username: string;
    menuImg = '';
    constructor(private globalVarService: GlobalVarService)
    {
    }

    ngOnInit()
    {
        this.username = this.globalVarService.username;
        // Change of each user
        this.menuImg = 'http://www.fogatadelpixel.com.ar/wp-content/uploads/2016/03/gow4-primary-teaser-horizontal-final.jpg';
    }

}
