import { Component, OnInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { timeout } from 'rxjs/operator/timeout';
import { GlobalVarService } from '../../_services/global-var.service';
import { WebServiceClientService } from '../../_services/web-service-client.service';
import { MethodPay } from '../../_models/method-pay';
import { Payment } from '../../_models/payment';
import { Router, ActivatedRoute } from '@angular/router';
import { ProgressService } from '../../_services/progress.service';

@Component({
    selector: 'app-payment',
    templateUrl: './payment.component.html',
    styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit
{
    listCatalogue: any = {};
    typeOfPayment: any = {};
    listCurrencies: any = {};
    // tslint:disable-next-line:no-inferrable-types
    result: boolean = false;
    message: string = '';
    isEmpty: boolean = false;
    loading: boolean = false;
    routeImage: string = '';
    progress: any;

    @ViewChild('fileInput') fileInput: ElementRef;


    constructor(private globalVarService: GlobalVarService,
                private webServiceClient: WebServiceClientService,
                public methodPay: MethodPay,
                private router: Router,
                private progressService: ProgressService,
                private zone: NgZone,
                private route: ActivatedRoute)
    {
        /*
                route.params.subscribe(p =>
        {
            // this.globalVarService.listOfPay[0].PaymentValue.Id = +p['id'];
        });
        */
    }

    ngOnInit()
    {
        const now = new Date();

        if (this.globalVarService.operation === 'Add')
        {
            this.methodPay = new MethodPay();
            this.methodPay.PaymentValue = new Payment();
            this.methodPay.PaymentValue.DatePay = now.toLocaleString();
            this.methodPay.PaymentValue.Timezone = this.globalVarService.timezone;
        }
        else if (this.globalVarService.operation === 'Edit' || this.globalVarService.operation === 'Delete')
        {
            this.globalVarService.firstUpload = false;
            this.methodPay = this.globalVarService.oldMethodPay;
            this.routeImage = this.getPreview(this.globalVarService.oldMethodPay.PaymentValue.Support);
        }
        this.webServiceClient.getWithAuth('values/GetTimeAsync').subscribe(tp =>
        {
            this.typeOfPayment = tp;
        });
        this.webServiceClient.getWithAuth('values/GetCataloguesAsync').subscribe(catalogue =>
        {
            this.listCatalogue = catalogue;
        });
        this.webServiceClient.getWithAuth('values/GetCurrencyAsync').subscribe(currency =>
        {
            this.listCurrencies = currency;
        });
    }
    getPreview(value): any
    {
        const url = this.globalVarService.urlRoot + '/upload/' + value;
        return url;
    }

    uploadPhoto()
    {
        const nativeElement: HTMLInputElement = this.fileInput.nativeElement;
        this.progressService.startTrackingUpload().subscribe(progress =>
        {
            this.zone.run(() =>
            {
                this.progress = progress;
            });
        }, null,
        () =>
        {
            this.progress = null;
        });
        this.webServiceClient.uploadSupport('Pictures', nativeElement.files[0]).subscribe(photo =>
        {
            this.methodPay.PaymentValue.Support = photo.data;
            this.routeImage = `${this.globalVarService.urlRoot}/upload/${this.methodPay.PaymentValue.Support}`;
            // this.globalVarService.oldMethodPay = this.methodPay;
            this.globalVarService.firstUpload = false;
        },
        err =>
        {
            if (err.status === 404)
            {
                console.log('Error: ' + err);
            }
        });
    }

    sendPayment()
    {
        if (this.globalVarService.operation === 'Edit')
        {
            this.webServiceClient.putData(`Budget/${this.globalVarService.typePaymentOriginal}`, this.methodPay).subscribe(result =>
            {
                alert(result.data);
                this.globalVarService.isUpdated = true;
                this.router.navigate(['/payment/list']);
            },
            err =>
            {
                if (err.status === 404)
                {
                    console.log('Error: ' + err);
                }
            });
        }
        else if (this.globalVarService.operation === 'Add')
        {
            this.webServiceClient.postData('Budget/PostPay', this.methodPay).subscribe(paymentResult =>
            {
                this.message = paymentResult.data;
                this.result = true;
                this.routeImage = '';
                // tslint:disable-next-line:prefer-const
                let nativeElement: HTMLInputElement = this.fileInput.nativeElement;
                nativeElement.form.reset();
                this.globalVarService.firstUpload = true;
            },
            err =>
            {
                if (err.status === 404)
                {
                    console.log('Error: ' + err);
                }
            });
        }
    }
}
