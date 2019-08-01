class Events {

    constructor(){
        this.listeners = {};
    }

    on(event, fn){
        if (!this.listeners[event]) {
            this.listeners[event] = [];
        }
        this.listeners[event].push(fn);
    }

    trigger(event){
        this.listeners[event].forEach(fn => fn());
    }

}

class ApiService {
    static callApi(url, method) {

        const options = {
            method,
        };

        return fetch(url, options)
            .then(response =>
                response.ok ? response.json() : Promise.reject(Error('Failed to load'))
            )
            .catch(error => {
                throw error;
            });
    }
}

class CollectionService {

    constructor(){
        this.API_URL = 'https://jsonplaceholder.typicode.com/photos';
    }

    async getCollection() {

        try {
            return await ApiService.callApi(this.API_URL, 'GET');

        } catch (error) {
            throw error;
        }
    }
}


class Collection extends Events {

    constructor(){
        super();
        this.list = [
            {
            "title": "Easily Customised",
            "thumbnailUrl": "img/Layer 48.png"
            },
            {
            "title": "Responsive Ready",
            "thumbnailUrl": "img/Layer 44.png"
            },
            {
            "title": "Modern Design",
            "thumbnailUrl": "img/Layer 47.png"
            },
            {
            "title": "Clean Code",
            "thumbnailUrl": "img/Layer 46.png"
            },
            {
            "title": "Ready to Ship",
            "thumbnailUrl": "img/Layer 43.png"
            },
            {
            "title": "Download For Free",
            "thumbnailUrl": "img/Layer 45.png"
            },
        ];
    }

    add(list) {
        this.list = list;
        this.trigger('change');
    }

    set _list(_list){
        this.list = _list;
    }


    get _list(){
        return this.list.slice();
    }
}

class Features {

    

    constructor() {
    this.templateFeatures = document.querySelector('#templateFeatures').content;

    this.model = new Collection();

    this.root = null;

    this._eventsAssign()
        ._render();
    }

    _eventsAssign() {

        document.querySelector('#btnFndOutMore').addEventListener('click', this.addItems.bind(this));

        this.model.on('change', this._render.bind(this));
        return this;
    }

    _render() {
        document.querySelector('#features').innerHTML = '';

        this.model._list.forEach((el, i) => {
            const template = this.templateFeatures.cloneNode(true);
            template.querySelector('._insImg').src = el["thumbnailUrl"];
            template.querySelector('._insTitle').innerHTML = el["title"];
            document.querySelector('#features').appendChild(template);
        });
    }

    async addItems(){

        const collectionService  = new CollectionService();

        const tmpList =  await collectionService.getCollection();

        const listRandom = [];
        for (let index = 0; index < this.model._list.length; index++) {
            listRandom.push(tmpList[Math.floor(Math.random() * tmpList.length)]);
        }

        this.model.add(listRandom);

    }

}




class FastScroll{

    constructor(menuItemSelector){
        this.anchors = [].slice.call(document.querySelectorAll(/*'._menuScroll'*/menuItemSelector));
        this.animationTime = 600;
        this.framesCount = 300;

        this.anchors.forEach(function(item) {
            item.addEventListener('click', function(e){               
                e.preventDefault();
                this._scroll.bind(item)(this.animationTime, this.framesCount);
            }.bind(this));
        }.bind(this));
    }

    _scroll(animationTime, framesCount){

        let coordY = document.querySelector(this.getAttribute('data-dest')).getBoundingClientRect().top;


        let scroller = setInterval(function() {

            let scrollBy = coordY / framesCount;

            if(scrollBy > window.pageYOffset - coordY && window.innerHeight + window.pageYOffset < document.body.offsetHeight) {
                window.scrollBy(0, scrollBy);
            } else {
                window.scrollTo(0, coordY);
                clearInterval(scroller);
            }
        }, animationTime / framesCount);
    }

}

new Features();

new FastScroll('._menuScroll');

$(document).ready(function(){
    $('._container').slick({
        dots: true,
        infinite: true,
        speed: 300,
        slidesToShow: 1,
        adaptiveHeight: true,
        arrows : false,
        fade: true,
    });
});