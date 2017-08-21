import { Component, OnInit } from '@angular/core';
import { VisibleService } from '../../_services/visible.service';
import { GlobalVarService } from '../../_services/global-var.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit
{
    constructor(public visibleService: VisibleService, public globalVarService: GlobalVarService)
    {

    }

    ngOnInit()
    {
    }

    private setOperationPayment(operation: string)
    {
        this.globalVarService.operation = operation;
    }
}
