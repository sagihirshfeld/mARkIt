class Marker {
    constructor(markData, markerLocation) {
        this.markData = markData;
        this.deleted = false;
        this.markerLocation = markerLocation;
        this.seen = false;
        this.drawables = [];
        this.onClickMark = this.onClickMark.bind(this);
        this.onEnterFieldOfVisionMark = this.onEnterFieldOfVisionMark.bind(this);
        this.initSignDrawble();
        this.initLabel();
        this.initIndicatorDrawable()
        this.initMarkGeoObject();    
    }

     initSignDrawble() {
        let markerImage;
        if (this.markData.style == "Wood") {
            markerImage = new AR.ImageResource("assets/woodSign.png");
        } else if (this.markData.style == "Metal") {
            markerImage = new AR.ImageResource("assets/metalSign.png");             
        } else {
            markerImage = new AR.ImageResource("assets/schoolSign.png");      
        }
    
        this.drawables.push(new AR.ImageDrawable(markerImage, 2.5, {
            zOrder: 0,
            opacity: 1.0
        }));
     }

     initLabel() {
        for(let i = 0; i < this.markData.message.length/14 ; i++) {
            this.drawables.push(new AR.Label(this.markData.message.substr( i * 14 ,14), 0.5, {
                zOrder: 1,
                style: {
                    textColor: '#FFFFFF'
                },
                offsetY: 0.5 - 0.5*i
            }));
        }
     }

     initIndicatorDrawable() {       
        let indicatorImage = new AR.ImageResource("assets/indi.png");
        this.indicatorDrawable = new AR.ImageDrawable(indicatorImage, 0.1, {
            verticalAnchor: AR.CONST.VERTICAL_ANCHOR.TOP
        });
     }

     onClickMark() {
        AR.platform.sendJSONObject({ "option": "rate", "markId": this.markData.id });
     }

     onEnterFieldOfVisionMark() {
        if (this.seen == false) {
            this.seen = true;
            AR.platform.sendJSONObject({ "option": "seen", "markId": this.markData.id });
        }
     }
     initMarkGeoObject() {
        this.markerObject = new AR.GeoObject(this.markerLocation, {
            drawables: {
                cam: this.drawables,
                indicator: [this.indicatorDrawable]
            },
            onClick: this.onClickMark,
            onEnterFieldOfVision: this.onEnterFieldOfVisionMark      
        });
     }
}


