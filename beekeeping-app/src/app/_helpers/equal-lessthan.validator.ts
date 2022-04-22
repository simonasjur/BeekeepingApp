import { AbstractControl, FormGroup } from '@angular/forms';

export function EqualLessthanValidator(num1: string, num2: string) {
    return (group: AbstractControl) => {
        const formGroup = <FormGroup>group;
        const number1Control = formGroup.controls[num1];
        const number2Control = formGroup.controls[num2];
        
        if (number1Control.errors || (number2Control.errors && !number2Control.errors.equalLessthan)) {
            return null;
        }

        const number1 = +number1Control.value;
        const number2 = +number2Control.value;

        if (number1 < number2) {
            number2Control.setErrors({ 'equalLessthan': true });
        } else {
            //Deletes laterDate error
            if (number2Control?.errors && number2Control?.errors['equalLessthan']) {
                delete number2Control.errors['equalLessthan'];
                if (Object.keys(number2Control.errors).length === 0) {
                    number2Control.setErrors(null);
                }
            }
        }
        return null;
    }
}