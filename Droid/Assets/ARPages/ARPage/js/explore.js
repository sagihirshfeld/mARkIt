
var m_lat, m_lon, m_alt, m_acc;
var m_LocationChanged = false;
var m_ShowedMarks = [];
var m_Marks = [];
var m_MarksLoaded = false;

const locationChanged = (lat, lon, alt, acc) => {
    m_lat = lat;
    m_lon = lon;
    m_acc = acc;
    m_alt = alt;
    if (!m_LocationChanged) {
        m_LocationChanged = true;
        getMarks();
        setInterval(getMarks, 30000);
    }

    if (m_MarksLoaded) {
        deleteOldMarks();
        showMarks();
    }
}

AR.context.onLocationChanged = locationChanged;

function setMarks(marksList) {
    m_Marks = null;
    m_Marks = marksList;
    m_MarksLoaded = true;
    locationChanged(m_lat,m_lon,m_alt,m_acc);
}

function getMarks() {
   AR.platform.sendJSONObject({ "option": "getMarks", "longitude": m_lon, "latitude": m_lat });   
}

function markIsShowed(mark){  
    for (let i = 0 ; i < m_ShowedMarks.length ; i++) {
        if(mark.id == m_ShowedMarks[i].markData.id && m_ShowedMarks[i].deleted == false)
            return true;
    }
    return false;
}

function markInNewList(mark){ 
    for (let i = 0 ; i < m_Marks.length ; i++) {      
        if(m_Marks[i].id == mark.id){
            return true;
        }        
    }  
    return false;
  
}

function deleteOldMarks(){
    for (let i = 0 ; i < m_ShowedMarks.length ; i++) {
        const markData = m_ShowedMarks[i].markData;
        if( (markInNewList(markData) == false) || (m_ShowedMarks[i].markerLocation.distanceToUser() > 40)){     
            m_ShowedMarks[i].markerObject.enabled = false;
            m_ShowedMarks[i].deleted = true;
            m_ShowedMarks.splice(i, 1);
            i--;
        }
    }
}

function showMarks(){
    for (let i = 0 ; i < m_Marks.length ; i++) {
        const mark = m_Marks[i];
        const altitude = m_alt ? m_alt + Math.random()*2 - Math.random()*2 : AR.CONST.UNKNOWN_ALTITUDE;
        let markerLocation = new AR.GeoLocation(mark.Latitude, mark.Longitude);     

        if((!markIsShowed(mark)) && (markerLocation.distanceToUser() <= 40)) {
              let markData = {
                  "id": mark.id,
                  "longitude": mark.Longitude,
                  "latitude": mark.Latitude,
                  "altitude": altitude,
                  "message": mark.Message,
                  "style": mark.Style

             };
             m_ShowedMarks.push(new Marker(markData,markerLocation)); 
         }
      }
}