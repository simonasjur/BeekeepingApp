import { AbstractControl, FormGroup } from '@angular/forms';

export function DatesValidator(from: string, to: string) {
    return (group: AbstractControl) => {
        const formGroup = <FormGroup>group;
        const firstDateControl = formGroup.controls[from];
        const secondDateControl = formGroup.controls[to];
        
        let firstDate: string;
        let secondDate: string;
        //Converst first date to string
        if (typeof firstDateControl.value === 'object') {
            if (firstDateControl.value) {
                firstDate = firstDateControl.value.toISOString()
            } else {
                return null;
            }
        } else {
            firstDate = firstDateControl.value;
        }
        //Converts second date to string
        if (typeof secondDateControl.value === 'object') {
            if (secondDateControl.value) {
                secondDate = secondDateControl.value.toISOString()
            } else {
                return null;
            }
        } else {
            secondDate = secondDateControl.value;
        }
        
        if (firstDateControl.errors || (secondDateControl.errors && !secondDateControl.errors.laterDate)) {
            return null;
        }

        if (firstDate > secondDate) {
            secondDateControl.setErrors({ 'laterDate': true });
        } else {
            //Deletes laterDate error
            if (secondDateControl?.errors && secondDateControl?.errors['laterDate']) {
                delete secondDateControl.errors['laterDate'];
                if (Object.keys(secondDateControl.errors).length === 0) {
                    secondDateControl.setErrors(null);
                }
            }
        }

        return null;
    }
}