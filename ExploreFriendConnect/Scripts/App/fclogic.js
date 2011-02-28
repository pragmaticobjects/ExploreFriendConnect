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

function init(times) { console.log('init ' + times); }

function onInitPage(st) { console.log('onInitPage ' + st); }

function onFriendConnectInitialized(st) {
    console.log('onFriendConnectInitialized ' + st);
  if (!window.friendconnect_times_loaded) {
    window.friendconnect_times_loaded = 1;
  } else {
    window.friendconnect_times_loaded++;
  }
  init(window.friendconnect_times_loaded);
  onInitPage(st);
}

function onAjaxLoaded() {
    console.log('onAjaxLoaded siteroot =' + siteRoot);
  google.friendconnect.container.setParentUrl(siteRoot);
  google.friendconnect.container.loadOpenSocialApi({
    site: siteid,
    onload: onFriendConnectInitialized
  });
}

function addFCLoginButton() {
  google.friendconnect.renderSignInButton({
      "id" : "login-fcbutton",
      "text" : "Sign in with Google Friend Connect",
      "style" : "long"
      });
}

function renderMembersGadget(divId, siteId) {
  var skin = {};
  skin['ENDCAP_BG_COLOR'] = '#eee';
  skin['ENDCAP_LINK_COLOR'] = '#36b';
  skin['CONTENT_LINK_COLOR'] = '#36b';
  skin['CONTENT_SECONDARY_LINK_COLOR'] = '#36b';

  skin['HEIGHT'] = '385';
  skin['BORDER_COLOR'] = '#cccccc';
  skin['ENDCAP_TEXT_COLOR'] = '#333333';
  skin['ALTERNATE_BG_COLOR'] = '#ffffff';
  skin['CONTENT_BG_COLOR'] = '#ffffff';
  skin['CONTENT_TEXT_COLOR'] = '#333333';
  skin['CONTENT_SECONDARY_TEXT_COLOR'] = '#666666';
  skin['CONTENT_HEADLINE_COLOR'] = '#333333';
  google.friendconnect.container.renderMembersGadget(
   { id: divId,
     site: siteId },
    skin);
}

function addFCViewFriendsButton() {
  google.friendconnect.renderSignInButton({
      "id" : "login-fcbutton",
      "text" : "View your friends on this site",
      "style" : "standard"
      });
}
