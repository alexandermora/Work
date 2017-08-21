import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalVarService } from '../../_services/global-var.service';
import { WebServiceClientService } from '../../_services/web-service-client.service';
import { MethodPay } from '../../_models/method-pay';

@Component({
    selector: 'app-preview-delete',
    templateUrl: './preview-delete.component.html',
    styleUrls: ['./preview-delete.component.css']
})
export class PreviewDeleteComponent implements OnInit
{
    private methodPayDelete: MethodPay;
    private messageDelete: string = 'Are you sure to delete this payment';
    private urlImage: string;

    constructor(private webServiceClient: WebServiceClientService, private globalVarService: GlobalVarService, private router: Router)
    {
        //
    }

    ngOnInit()
    {
        this.methodPayDelete = this.globalVarService.oldMethodPay;
        this.urlImage = this.getPreview(this.methodPayDelete.PaymentValue.Support);
    }
    deletePayment()
    {
        this.webServiceClient.deleteData(`Budget/${this.methodPayDelete.PaymentValue.Id}/${this.methodPayDelete.TypePayment}`).subscribe(result =>
        {
            alert(result.data);
            this.router.navigate(['/payment/list']);
        });
    }
    returnList()
    {
        this.router.navigate(['/payment/list']);
    }
    getPreview(value): any
    {
        const url = this.globalVarService.urlRoot + '/upload/' + value;
        return url;
    }

}
