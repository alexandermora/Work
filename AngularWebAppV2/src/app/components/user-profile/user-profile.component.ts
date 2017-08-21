import { GlobalVarService } from '../../_services/global-var.service';
import { Component, OnInit } from '@angular/core';
import { WebServiceClientService } from '../../_services/web-service-client.service';

@Component({
    selector: 'app-user-profile',
    templateUrl: './user-profile.component.html',
    styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit
{

    constructor(private glovalVarService: GlobalVarService, private webServiceClient: WebServiceClientService)
    {
        // /
    }

    ngOnInit()
    {
    }

}
