import { Component, OnInit } from '@angular/core';
import { DateModel, DatePickerOptions } from 'ng2-datepicker';
import { GlobalVarService } from '../../_services/global-var.service';
import { WebServiceClientService } from '../../_services/web-service-client.service';

@Component({
    selector: 'app-chart-payment',
    templateUrl: './chart-payment.component.html',
    styleUrls: ['./chart-payment.component.css']
})
export class ChartPaymentComponent implements OnInit
{
    date: DateModel;
    options: DatePickerOptions;
    listFilter: any = {};
    typeOfPayment: any = {};
    typeConsult: any = {};
    haveData: boolean = false;
    selectedFilter: string;
    selectedTypeOfPayment: string;
    selectedTypeConsult: string;
    nameList: Array<string>;
    valueList: Array<number>;
    pieChartType: string = 'pie';

    constructor(private glovalVarService: GlobalVarService, private webServiceClientService: WebServiceClientService)
    {
    }

    ngOnInit()
    {
        this.webServiceClientService.getWithAuth('values/GetTimeAsync').subscribe(tp =>
        {
            this.typeOfPayment = tp;
        });
        this.webServiceClientService.getWithAuth('values/FilterAsync').subscribe(f =>
        {
            this.listFilter = f;
        });
        this.webServiceClientService.getWithAuth('values/TypeConsultAsync').subscribe(tf =>
        {
            this.typeConsult = tf;
        });
    }
    getChart()
    {
        const url = `Budget/GetPaymentFilteredAsync/${this.selectedFilter}/${this.date.formatted.toString()}/${this.selectedTypeOfPayment}/${this.selectedTypeConsult}`;
        this.webServiceClientService.getWithAuth(url).subscribe(result =>
        {
            this.nameList = new Array<string>();
            this.valueList = new Array<number>();
            for (const valueR of result.listDto)
            {
                this.nameList.push(valueR.name);
                this.valueList.push(valueR.count);
            }
            this.haveData = true;
        });
    }
    public chartClicked(e: any): void
    {
        console.log(e);
    }

    public chartHovered(e: any): void
    {
        console.log(e);
    }
}
