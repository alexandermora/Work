import { Component, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { forEach } from '@angular/router/src/utils/collection';
import { GlobalVarService } from '../../_services/global-var.service';
import { Payment } from '../../_models/payment';
import { WebServiceClientService } from '../../_services/web-service-client.service';
import { MethodPay } from '../../_models/method-pay';




@Component({
  selector: 'app-list-payment',
  templateUrl: './list-payment.component.html',
  styleUrls: ['./list-payment.component.css']
})
export class ListPaymentComponent implements OnInit
{
    dateUser: any;
    typeOfPayment: any = {};
    selectedTypeOfPayment: string;
    isEmpty = true;
    loading = false;
    listPaymentSelected: any = {};
    constructor(private webServiceClient: WebServiceClientService, private globalVarService: GlobalVarService, private router: Router)
    {
    }

    ngOnInit()
    {
        this.webServiceClient.getWithAuth('values/GetTimeAsync').subscribe(tp =>
        {
            this.typeOfPayment = tp;
        });
        if (this.globalVarService.isUpdated)
        {
            this.dateUser = this.globalVarService.dateSelected;
            this.selectedTypeOfPayment = this.globalVarService.typePaymentOriginal;
            this.searchData();
        }
    }
    searchData()
    {

        this.webServiceClient.getWithAuth(`Budget/${this.selectedTypeOfPayment}/${this.dateUser}`).subscribe(listPayment =>
        {
            this.globalVarService.dateSelected = this.dateUser;
            this.listPaymentSelected = listPayment.listDto;
            this.isEmpty = false;
            this.globalVarService.isUpdated = false;
        },
        err =>
        {
            console.log(err);
        });
    }
    updatePayment(p)
    {
        this.globalVarService.oldMethodPay = new MethodPay();
        this.globalVarService.oldMethodPay.PaymentValue = new Payment();
        this.globalVarService.oldMethodPay.TypePayment = this.selectedTypeOfPayment;
        this.globalVarService.typePaymentOriginal = this.selectedTypeOfPayment;
        this.globalVarService.oldMethodPay.PaymentValue.Id = p.paymentValue.id;
        this.globalVarService.oldMethodPay.PaymentValue.Name = p.paymentValue.name;
        this.globalVarService.oldMethodPay.PaymentValue.Description = p.paymentValue.description;
        this.globalVarService.oldMethodPay.PaymentValue.TypeCatalogue = p.paymentValue.typeCatalogue;
        this.globalVarService.oldMethodPay.PaymentValue.ValuePay = p.paymentValue.valuePay;
        this.globalVarService.oldMethodPay.PaymentValue.Currency = p.paymentValue.currency;
        this.globalVarService.oldMethodPay.PaymentValue.DatePay = p.paymentValue.datePay;
        this.globalVarService.oldMethodPay.PaymentValue.Timezone = p.paymentValue.timezone;
        this.globalVarService.oldMethodPay.PaymentValue.Support = p.paymentValue.support;
        this.globalVarService.operation = 'Edit';
        this.router.navigate(['/payment/edit']);
    }

    deletePayment(p)
    {
        this.globalVarService.oldMethodPay = new MethodPay();
        this.globalVarService.oldMethodPay.PaymentValue = new Payment();
        this.globalVarService.oldMethodPay.TypePayment = this.selectedTypeOfPayment;
        this.globalVarService.typePaymentOriginal = this.selectedTypeOfPayment;
        this.globalVarService.oldMethodPay.PaymentValue.Id = p.paymentValue.id;
        this.globalVarService.oldMethodPay.PaymentValue.Name = p.paymentValue.name;
        this.globalVarService.oldMethodPay.PaymentValue.Description = p.paymentValue.description;
        this.globalVarService.oldMethodPay.PaymentValue.TypeCatalogue = p.paymentValue.typeCatalogue;
        this.globalVarService.oldMethodPay.PaymentValue.ValuePay = p.paymentValue.valuePay;
        this.globalVarService.oldMethodPay.PaymentValue.Currency = p.paymentValue.currency;
        this.globalVarService.oldMethodPay.PaymentValue.DatePay = p.paymentValue.datePay;
        this.globalVarService.oldMethodPay.PaymentValue.Timezone = p.paymentValue.timezone;
        this.globalVarService.oldMethodPay.PaymentValue.Support = p.paymentValue.support;
        this.globalVarService.operation = 'Delete';
        this.router.navigate(['/payment/delete']);
    }
    getChart()
    {
        this.router.navigate(['/payment/grapth']);
    }
    getPreview(value): any
    {
        const url = this.globalVarService.urlRoot + '/upload/' + value;
        return url;
    }
}
