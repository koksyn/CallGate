import TypeCheckService from "./TypeCheckService";

export default {
    getArrayOfErrorsFromResponse(response) {
        return this.getArrayOfErrorsFromVariable(response.data);
    },
    getArrayOfErrorsFromVariable(variable) {
        let errors = [];
        let outside = this;
        
        if (TypeCheckService.isString(variable)) {
            errors.push(variable);
        } 
        else if (TypeCheckService.isArray(variable)) {
            variable.forEach(function (field, index) {
                let localErrors = outside.getArrayOfErrorsFromVariable(field);
                
                localErrors.forEach(function (error, i) {
                    errors.push(error);
                });
            });
        }
        else if (TypeCheckService.isObject(variable)) {
            Object.keys(variable).map(function(objectKey, index) {
                let value = variable[objectKey];
                
                let localErrors = outside.getArrayOfErrorsFromVariable(value);
                localErrors.forEach(function (error, i) {
                    errors.push(error);
                });
            });
        }
        
        return errors;
    }
}