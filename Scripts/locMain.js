var businesses = [
{name: 'Bayville VFW',
street: '383 Veterans Blvd',
city: 'Bayville',
state: 'NJ',
zip: '08721',
country: 'Ocean',
lat: 39.891069, lng: -74.190269},
{
name: 'The Sportsmen\'s Center',
street: '69 US Highway 130 N',
city: 'Bordentown',
state: 'NJ',
zip: '08505',
country: 'Burlington',
lat: 40.150292, lng: -74.700533},
{name: 'Findern Fire House',
street: '672 East Main Street',
city: 'Bridgewater',
state: 'NJ',
zip: '08807',
country: 'Somerset',
lat: 40.562574, lng: -74.573074},
{name: 'Clifton VFW',
street: '491 Valley Road',
city: 'Clifton',
state: 'NJ',
zip: '07013',
country: 'Passaic',
lat: 40.882663, lng: -74.180313}
,
{name: 'Dunellen Knights of Columbus',
street: '647 Grove Street',
city: 'Dunellen',
state: 'NJ',
zip: '08812',
country: 'Middlesex',
lat: 40.588651, lng: -74.465014}
,
{name: 'Somerset Hills American Legion',
street: '151 US Hwy 206',
city: 'Gladstone',
state: 'NJ',
zip: '07934',
country: 'Somerset',
lat: 40.714558, lng: -74.672917}
,
{name: 'Glassboro VFW',
street: '275 Wilmer St. (corner of Wilmer & Sewell)',
city: 'Glassboro',
state: 'NJ',
zip: '08028',
country: 'Gloucester',
lat: 39.7023, lng: -75.120075}
,
{name: 'St. Mary\'s Church',
street: '1900 Brook Blvd',
city: 'Hillsborough',
state: 'NJ',
zip: '08844',
country: 'Somerset',
lat: 40.542423, lng: -74.609381}
,
{name: 'Keyport Matawan Elks',
street: '249 Broadway',
city: 'Keyport',
state: 'NJ',
zip: '07735',
country: 'Monmouth',
lat: 40.42993, lng: -74.210182}
,
{name: 'Leonardo American Legion',
street: '860 State Route 36',
city: 'Leonardo',
state: 'NJ',
zip: '07737',
country: 'Monmouth',
lat: 40.415636, lng: -74.063154}
,
{name: 'Magnolia American Legion',
street: '430 N. Warwick Road',
city: 'Magnolia',
state: 'NJ',
zip: '08049',
country: 'Camden',
lat: 39.859468, lng: -75.033556}
,
{name: 'Mountainside Elks',
street: '1193 US Highway 22 East',
city: 'Mountainside',
state: 'NJ',
zip: '07092',
country: 'Union',
lat: 40.678327, lng: -74.345001}
,
{name: 'Morris County Public Safety Training Academy',
street: '500 West Hanover Ave',
city: 'Parsippany',
state: 'NJ',
zip: '07054',
country: 'Morris',
lat: 40.848132, lng: -74.424652}
,
{name: 'South Brunswick VFW',
street: '11 Henderson Road',
city: 'South Brunswick',
state: 'NJ',
zip: '08824',
country: 'Middlesex',
lat: 40.446425, lng: -74.54303}
,
{name: 'So Plainfield American Legion',
street: '243 Oak Tree Ave',
city: 'South Plainfield',
state: 'NJ',
zip: '07080',
country: 'Middlesex',
lat: 40.579868, lng: -74.404675}
,
{name: 'South River VFW',
street: '31 Reid Street',
city: 'South River',
state: 'NJ',
zip: '08882',
country: 'Middlesex',
lat: 40.453658, lng: -74.381114}
,
{name: 'Stirling Elks',
street: '1138 Valley Road',
city: 'Stirling',
state: 'NJ',
zip: '07980',
country: 'Morris',
lat: 40.668595, lng: -74.487694}
,
{name: 'Toms River Elks',
street: '600 Washington Street',
city: 'Toms River',
state: 'NJ',
zip: '08753',
country: 'Ocean',
lat: 39.953109, lng: -74.176107}
,
{name: 'Washington Valley Fire Dept',
street: '146 Washington Valley Rd',
city: 'Warren',
state: 'NJ',
zip: '07059',
country: 'Somerset',
lat: 40.609114, lng: -74.501062}
,
{name: 'Westfield Knights of Columbus',
street: '2400 North Avenue',
city: 'Westfield',
state: 'NJ',
zip: '07076',
country: 'Union',
lat: 40.647808, lng: -74.365815}
,
{name: 'Ocean Explorers Aquatic Center',
street: '180 Lafayette Ave.',
city: 'Edison',
state: 'NJ',
zip: '08837',
country: 'Middlesex',
lat: 40.544575, lng: -74.334122}
];

/**
 * The map object, null until script loads in.
 * @type {GMap2}
 */
var map = null;  

/**
 * The bounds of the markers once loaded in.
 * @type {GLatLngBounds}
 */
var bounds = null;

/**
 * The marker with currently opened info window.
 * @type {GMarker}
 */
var currentMarker = null;

/**
 * The dom element that the map is loaded into
 * @type {Element}
 */
var mapDiv = null;

/**
 * The dom element that everything is a child of.
 * @type {Element}
 */
var containerDiv = null;

/**
 * Position of mouse click (clientX) on map div when in static mode.
 * @type {Number}
 */
var clickedX = 0;

/**
 * Position of mouse click (clientY) on map div when in static mode.
 * @type {Number}
 */
var clickedY = 0;

/**
 * Indicates whether we've created a script tag with Maps API yet
 * @type {Boolean}
 */
var isLoaded = false;

// Create a base icon for all of our markers that specifies the
// shadow, icon dimensions, etc.
//var baseIcon = new GIcon();
//baseIcon.shadow = "http://www.google.com/mapfiles/shadow50.png";
//baseIcon.iconSize = new GSize(30, 30);
//baseIcon.shadowSize = new GSize(37, 34);
//baseIcon.iconAnchor = new GPoint(9, 34);
//baseIcon.infoWindowAnchor = new GPoint(9, 2);
//baseIcon.infoShadowAnchor = new GPoint(18, 25);

//var trackIcon = new google.maps.Icon({
//    url :"/images/www2/icons/map_track_marker.png",
//    size : new google.maps.Size(6,6),
//    anchor : new google.maps.Point(3,3)
//});

/**
 * Called after script is asynchronously loaded in.
 * Creates the GMap2, GMarker objects and performs actions according to 
 * what the user did to trigger the map load (search, zoom, click etc).
 */
function loadMap() {
containerDiv = document.getElementById('container');
  mapDiv = document.getElementById('map');
  
  var mapOptions = {
        zoom: 9,
        center: new google.maps.LatLng(40.453658, -74.381114),
        mapTypeId: google.maps.MapTypeId.ROADMAP
};
map = new google.maps.Map(mapDiv, mapOptions);


    for (var i = 0; i < businesses.length; i++) {
      createMarker(i);
    }

//  if (GBrowserIsCompatible()) {
//    mapDiv.style.background = '#fff';
//    mapDiv.style.cursor = '';
//    map = new GMap2(mapDiv, {logoPassive: true});
//    map.addControl(new GSmallMapControl());
//    map.addControl(new GMapTypeControl());
//    map.addControl(new GScaleControl());
//    
//    bounds = new GLatLngBounds();
//    for (var i = 0; i < businesses.length; i++) {
//      bounds.extend(new GLatLng(businesses[i].lat, businesses[i].lng));
//    }
//    var latSpan = bounds.toSpan().lat();
//    //map.setCenter(bounds.getCenter(), map.getBoundsZoomLevel(bounds));
//    map.setCenter(bounds.getCenter(), map.getBoundsZoomLevel(bounds));
//    // The static map server gives markers more space when calculating
//    // bounds and zoom level, so sometimes the API will give a higher
//    // zoom level than was used by the static map server.
//    // The .98 value is just a guess right now, may need tweaking.
//    var newBounds = map.getBounds();
//    var newLatSpan = newBounds.toSpan().lat();
//    //if (latSpan/newLatSpan > .90) { map.zoomOut(); }
// map.zoomOut();
//  map.zoomOut();
//   map.zoomOut();
//    map.zoomOut();
//     map.zoomOut();
//    for (var i = 0; i < businesses.length; i++) {
//      var marker = createMarker(i);
//      //var latlng = marker.getLatLng();
//      //var pixel = map.fromLatLngToDivPixel(latlng);
//      //if (Math.abs(pixel.x - clickedX) < 12 && Math.abs(pixel.y - clickedY) < 20) {
//        //GEvent.trigger(marker, 'click');
//      //}
//      map.addOverlay(marker);
//    }
//    
//  }
}



/**
 * Zooms to the viewport that fits all the markers.
 */
function zoomToAll() {
  map.setCenter(bounds.getCenter(), map.getBoundsZoomLevel(bounds));
}

/**
* Zooms to certain point
*/
function zoomPoint(lat, lng) {
    map.setCenter(new GLatLng(lat, lng), 17);
}

/**
 * Creates a marker for the given business.
 * @param {Number} ind
 * @return {GMarker}
 */
function createMarker(ind) {
  var business = businesses[ind];
  // Create a lettered icon for this point using our icon class
  //var letter = String.fromCharCode("A".charCodeAt(0) + ind);
  //var letteredIcon = new GIcon(baseIcon);
  //letteredIcon.image = "http://www.google.com/mapfiles/marker" + letter + ".png";
//letteredIcon.image = "http://pitmastersllc.adisites.com/images/piggy.png";

  // Set up our GMarkerOptions object
  //markerOptions = { icon:letteredIcon };
  
  
  var properties = {
        position: new google.maps.LatLng(business.lat, business.lng),
        map: map,
        title:business.name,
        icon:'http://labs.google.com/ridefinder/images/mm_20_red.png',
        shadow:'http://labs.google.com/ridefinder/images/mm_20_shadow.png'
};


var marker = new google.maps.Marker(properties); 
var contentString = ""; 
  
  //var marker = new GMarker(new GLatLng(business.lat, business.lng), markerOptions);
  
  google.maps.event.addListener(marker, 'click', function() {
    
    contentString = ['<span style=font-weight:bold;color:#F48026;>', business.name, '</span><br>', 
                         business.street, '<br> ', business.city, ', ', 
                         business.state, ' ', business.zip, ' ', business.country].join('');
    
    
    var infowindow = new google.maps.InfoWindow({
      content: contentString
  });
    
    currentMarker = marker;
    //marker.openInfoWindowHtml(marker.html);
    
    infowindow.open(map,marker);
    
  });
  return marker;
}

/**
 * Formats business info into a URL-friendly version for maps url.
 * @param {Object} business
 * @return {String}
 */
function formatAddressForMaps(business) {
  var address = business.street + ' ' + business.city + ' ' + business.state + ' ' + business.zip;
  return escape(address.replace(' ', '+'));
}

/**
 * Convenience function for creating an element and assigning an id to it.
 * @param {String} elementType
 * @param {String} id
 * @return {Element} 
 */
function _cel(elementType, id) {
  var element = document.createElement(elementType);
  element.id = id;
  return element;
}

/**
 * Loads in the Maps API script. This is called after some sort of user interaction.
 * The script loads asynchronously and calls loadMap once it's in.
 */
function loadScript() {
  if (!isLoaded) {
    isLoaded = true;
    var div = document.createElement('div');
    div.className = 'message';
    div.innerHTML = 'Loading...';
    div.style.left = (580/2 - 53) + 'px';
    div.style.top = 410/2 + 'px'; 
    mapDiv.appendChild(div);
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = 'http://maps.google.com/maps?file=api&v=2.x' + 
                 '&async=2&callback=loadMap&key=ABQIAAAArEEehrdiRzZmdc6MtS6GIRRkHR2NAkMD98aU-4LdRdzdDtnfEBSfsgCmfq2A6tzGIQyTgQMG4KlKEA';
    document.body.appendChild(script);
  }
}

/**
 * Sets up the gadget by setting CSS and click events.
 */
function loadMapGadget() {
  containerDiv = document.getElementById('container');
  mapDiv = document.getElementById('map');

  //mapDiv.onclick = function (e) {
   // clickedX = (window.event && window.event.offsetX) || e.clientX;
   // clickedY = (window.event && window.event.offsetY) || e.clientY;
    loadScript(); 
  //};

  /*mapDiv.style.cursor = 'pointer';

  var urlString = ['http://maps.google.com/staticmap?markers='];
  var markerString = [];
  for (var i = 0; i < businesses.length; i++) {
    markerString.push(businesses[i].lat + ',' + businesses[i].lng + ',red');
  }
  urlString.push(markerString.join('|'));
  urlString.push('&size=410x410');
  urlString.push('&key=ABQIAAAArEEehrdiRzZmdc6MtS6GIRRkHR2NAkMD98aU-4LdRdzdDtnfEBSfsgCmfq2A6tzGIQyTgQMG4KlKEA');
  mapDiv.style.background = 'url(\'' + urlString.join('') + '\')';
*/
}

/**
* Create location list
*/

