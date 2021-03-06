function appSerializeObject(object: any, forseNotValidate: boolean = false): any {
    if ($.isPlainObject(object)) {
        return object;
    }

    var serializeArray: JQuerySerializeArrayElement[];

    if ($.isArray(object)) {
        serializeArray = object;
    } else {
        const inputs = $(object).find(":input");
        serializeArray = inputs.serializeArray();

        inputs.filter("select.multiselect").each(function () {
            if (!$(this).val() || !$(this).val().length) {
                const name = $(this).attr("id");
                let nameExistInArray = false;
                for (let i = 0; i < serializeArray.length; i++) {
                    if (serializeArray[i].name === name) {
                        nameExistInArray = true;
                        break;
                    }
                }
                if (!nameExistInArray) {
                    serializeArray.push({
                        name: name,
                        value: "<CREATE_EMPTY_ARRAY>"
                    });
                }
            }
        });
    }
    var self = this,
        json:any = {},
        pushCounters: { [id: string]: number; } = {},
        patterns = {
            "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
            "key": /[a-zA-Z0-9_]+|(?=\[\])/g,
            "push": /^$/,
            "fixed": /^\d+$/,
            "named": /^[a-zA-Z0-9_]+$/
        };

    this.build = (base:any, key:string, value:any) => {
        base[key] = value;
        return base;
    };

    this.push_counter = (key:string): number => {
        if (pushCounters[key] === undefined) {
            pushCounters[key] = 0;
        }
        return pushCounters[key]++;
    };

    $.each(serializeArray, function () {
        if (!forseNotValidate) {
            if (!patterns.validate.test(this.name)) {
                return;
            }
        }

        var k: any,
            keys = this.name.match(patterns.key),
            merge = this.value,
            reverse_key = this.name;

        while ((k = keys.pop()) !== undefined) {
            reverse_key = reverse_key.replace(new RegExp("\\[" + k + "\\]$"), '');
            if (k.match(patterns.push)) {
                merge = self.build([], self.push_counter(reverse_key), merge);
            }
            else if (k.match(patterns.fixed)) {
                merge = self.build([], k, merge);
            }
            else if (k.match(patterns.named)) {
                merge = self.build({}, k, merge);
            }
        }

        if (this.value === "<CREATE_EMPTY_ARRAY>") {
            json[this.name] = [];
        } else {
            if ((this.name in json)) {
                if (!(Object.prototype.toString.call(json[this.name]) === '[object Array]')) {
                    json[this.name] = [json[this.name]];
                }
                json[this.name].push(this.value);
            } else {
                json = $.extend(true, json, merge);
            }
        }
    });

    return json;
}