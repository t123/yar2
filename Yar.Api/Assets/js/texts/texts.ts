﻿import Vue from 'vue';

var app = new Vue({
    el: '#app',
    data: {
        texts: [],
        filter: '',
        loading: true,
        timeout: null
    },
    mounted() {
        this.filter = window.localStorage.filter || '';
        this.getTexts();
    },
    destroyed() {
        if (this.timeout) {
            clearTimeout(this.timeout);
        }
    },
    methods: {
        filterByLanguage(language: string) {
            this.filter = language;
            this.getTexts()
        },
        getTexts() {
            if (this.timeout) {
                clearTimeout(this.timeout);
            }

            this.timeout = setTimeout(() => {
                window.localStorage.filter = this.filter;
                this.loading = true;

                fetch('/texts/search', {
                    method: 'POST',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ filter: this.filter })
                })
                    .then(response => response.json())
                    .then(json => {
                        this.texts = json;
                        this.loading = false;
                    });
            }, 500)
        },
        deleteText: function(textId) {
            this.loading = true;

            fetch(`/texts/delete/${textId}`, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
                .then(response => {
                    this.getTexts();
                    this.loading = false;
                });
        }
    }
});
