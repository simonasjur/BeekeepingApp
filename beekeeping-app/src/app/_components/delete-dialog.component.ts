import { Component, Inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
    selector: 'delete-dialog',
    templateUrl: 'delete-dialog.component.html',
  })
  export class DeleteDialog {
    constructor(
      public dialogRef: MatDialogRef<DeleteDialog>
    ) {}
}