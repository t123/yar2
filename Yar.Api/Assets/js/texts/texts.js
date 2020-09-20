"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var vue_1 = require("vue");
var app = new vue_1.default({
    el: '#app',
    data: {
        texts: [],
        filter: '',
        loading: true,
        timeout: null
    },
    mounted: function () {
        this.filter = window.localStorage.filter || '';
        this.getTexts();
    },
    destroyed: function () {
        if (this.timeout) {
            clearTimeout(this.timeout);
        }
    },
    methods: {
        filterByLanguage: function (language) {
            this.filter = language;
            this.getTexts();
        },
        getTexts: function () {
            var _this = this;
            if (this.timeout) {
                clearTimeout(this.timeout);
            }
            this.timeout = setTimeout(function () {
                window.localStorage.filter = _this.filter;
                _this.loading = true;
                fetch('/texts/search', {
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ filter: _this.filter })
                })
                    .then(function (response) { return response.json(); })
                    .then(function (json) {
                    _this.texts = json;
                    _this.loading = false;
                });
            }, 500);
        },
        deleteText: function (textId) {
            var _this = this;
            this.loading = true;
            fetch("/texts/delete/" + textId, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
                .then(function (response) {
                _this.getTexts();
                _this.loading = false;
            });
        }
    }
});
//# sourceMappingURL=texts.js.map