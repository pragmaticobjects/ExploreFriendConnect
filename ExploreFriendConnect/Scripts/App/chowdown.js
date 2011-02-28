/*
 * Copyright 2009 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

function setLocalLocation() {
  if (google.loader.ClientLocation) {
    loc = google.loader.ClientLocation.address;
    $("#local-location").val(loc.city + ", " + loc.region + ", " + loc.country);
  }
}

function setMessage(text) {
  document.getElementById("message").innerHTML = text;
  document.getElementById("message").className = "smallrounded";
}

function getFriendsAndRestaurants(start, count) {
  $.ajax({
    type: "POST",
    url: "ajax.aspx",
    data: { 
      "action" : "getFriendsAndRestaurants",
      "start" : start,
      "count" : count,
    },
    dataType: "text",
    success: function(data) {
      document.getElementById("fc-content").innerHTML = data;
    }
  });
}

function getViewerRestaurants() {
  $.ajax({
    type: "POST",
    url: "ajax.aspx",
    data: { 
      "action" : "getViewerRestaurants",
    },
    dataType: "text",
    success: function(data) {
      document.getElementById("labels-list").innerHTML = data;
    }
  });
}

function deleteViewerRestaurant(id) {
  $.ajax({
    type: "POST",
    url: "ajax.aspx",
    data: { 
      "action" : "deleteViewerRestaurant",
      "restaurantId" : id
    },
    dataType: "text",
    success: function(data) {
      getViewerRestaurants();
    }
  });
}

function getInvitesForViewer() {
  $.ajax({
    type: "POST",
    url: "ajax.aspx",
    data: { 
      "action" : "getInvitesForViewer",
    },
    dataType: "text",
    success: function(data) {
      document.getElementById("inviteslist").innerHTML = data;
    }
  });
}

function getFriendsForInvitation(restaurantId, start, count) {
  $.ajax({
    type: "POST",
    url: "ajax.aspx",
    data: { 
      "action" : "getFriendsForInvitation",
      "restaurantId" : restaurantId,
      "start" : start,
      "count" : count,
    },
    dataType: "text",
    success: function(data) {
      var results = JSON.parse(data);
      document.getElementById("invitefriends-list").innerHTML = results['contents'];
      document.getElementById("invitefriends-prev-td").innerHTML = results['prev'];
      document.getElementById("invitefriends-next-td").innerHTML = results['next'];
      if (invitefriends_selected == null) {
        invitefriends_selected = results['checklist'];
      }
      displayListOfInvitedFriends();
    }
  });
}

function sendInvitationToFriends() {
  userIds = [];
  for (var userId in invitefriends_selected) {
    userIds.push(userId);
  }
  $.ajax({
    type: "POST",
    url: "ajax.aspx",
    data: { 
      "action" : "sendInvitationToFriends",
      "restaurantId" : window.invitefriends_restaurantId,
      "userIds" : userIds.join(",")
    },
    dataType: "text",
    success: function(data) {
      hideInviteFriendsDialog();
      getViewerRestaurants();
    }
  });
}

function showInviteFriendsDialog(restaurantId) {
  window.invitefriends_restaurantId = restaurantId;
  invitefriends_selected = null;
  document.getElementById("invitefriends").className = "";
  getFriendsForInvitation(restaurantId, 0, 4);
}

function hideInviteFriendsDialog() {
  document.getElementById("invitefriends").className = "hidden";
}

function toggleInvitationForFriend(user_id) {
  var checkbox = document.getElementById("invitefriends-checkbox-" + user_id);
  var container = document.getElementById("invitefriends-friend-" + user_id);
  var guid = checkbox.value;
  if (checkbox.checked == true) {
    invitefriends_selected[guid] = document.getElementById("invitefriends-name-" + user_id).value;
  } else {
    delete invitefriends_selected[guid];
  }
  displayListOfInvitedFriends();
};

function displayListOfInvitedFriends() {
  names = [];
  for (var userId in invitefriends_selected) {
    names.push(invitefriends_selected[userId]);
  }
  if (names.length > 0) {
    document.getElementById("invitefriends-selected").innerHTML = "Inviting <strong>" + names.join("</strong>, <strong>") + "</strong>";
  } else {
    document.getElementById("invitefriends-selected").innerHTML = "";
  }
};

function fetchSearchResults() {
  $.ajax({
    type: "POST",
    url: "ajax.aspx",
    data: { 
      "action" : "fetchSearchResults",
      "query" : document.getElementById('local-term').value,
      "location" : document.getElementById('local-location').value,
    },
    dataType: "text",
    success: function(data) {
      document.getElementById("local-results").innerHTML = data;
      document.getElementById("local-results").className = "";
    }
  });
}

function closeSearchResults() {
  document.getElementById("local-results").className = "hidden";
}

function bookmarkRestaurant(id, title, address, url, phone, averagerating, 
    rating, latitude, longitude) {
  $.ajax({
    type: "POST",
    url: "ajax.aspx",
    data: { 
      "id" : id,
      "action" : "bookmarkRestaurant",
      "title" : title,
      "address" : address,
      "url" : url,
      "phone" : phone,
      "averagerating" : averagerating,
      "rating" : rating,
      "latitude" : latitude,
      "longitude" : longitude
    },
    dataType: "text",
    success: function(data) {
      getViewerRestaurants();
    }
  });  
}