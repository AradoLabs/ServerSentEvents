/** @license
 * eventsource.js
 * Available under MIT License (MIT)
 * https://github.com/Yaffle/EventSource/
 */
!function(a){"use strict";function b(){this.data={}}function c(){this.listeners=new b}function d(a){setTimeout(function(){throw a},0)}function e(a){this.type=a,this.target=null}function f(a,b){e.call(this,a),this.data=b.data,this.lastEventId=b.lastEventId}function g(a,b){var c=Number(a)||b;return z>c?z:c>A?A:c}function h(a,b,c){try{"function"==typeof b&&b.call(a,c)}catch(e){d(e)}}function i(b,d){function i(){L=s,null!==H&&(H.abort(),H=null),0!==I&&(clearTimeout(I),I=0),0!==J&&(clearTimeout(J),J=0),E.readyState=s}function j(a){var b=L===r||L===q?H.responseText||"":"",c=null,d=!1;if(L===q){var i=0,j="",k="";if(n)try{i=Number(H.status||0),j=String(H.statusText||""),k=String(H.getResponseHeader("Content-Type")||"")}catch(l){i=0}else i=200,k=H.contentType;if(200===i&&y.test(k)){if(L=r,G=!0,F=B,E.readyState=r,c=new e("open"),E.dispatchEvent(c),h(E,E.onopen,c),L===s)return}else if(0!==i){var m="";m=200!==i?"EventSource's response has a status "+i+" "+j.replace(/\s+/g," ")+" that is not 200. Aborting the connection.":"EventSource's response has a Content-Type specifying an unsupported type: "+k.replace(/\s+/g," ")+". Aborting the connection.",setTimeout(function(){throw new Error(m)}),d=!0}}if(L===r){b.length>K&&(G=!0);for(var o=K-1,z=b.length,J="\n";++o<z;)if(J=b[o],Q===t&&"\n"===J)Q=u;else if(Q===t&&(Q=u),"\r"===J||"\n"===J){if("data"===R?M.push(S):"id"===R?N=S:"event"===R?O=S:"retry"===R?(B=g(S,B),F=B):"heartbeatTimeout"===R&&(C=g(S,C),0!==I&&(clearTimeout(I),I=setTimeout(P,C))),S="",R="",Q===u){if(0!==M.length&&(D=N,""===O&&(O="message"),c=new f(O,{data:M.join("\n"),lastEventId:N}),E.dispatchEvent(c),"message"===O&&h(E,E.onmessage,c),L===s))return;M.length=0,O=""}Q="\r"===J?t:u}else Q===u&&(Q=v),Q===v?":"===J?Q=w:R+=J:Q===w?(" "!==J&&(S+=J),Q=x):Q===x&&(S+=J);K=z}L!==r&&L!==q||!(a||d||K>1048576||0===I&&!G)?0===I&&(G=!1,I=setTimeout(P,C)):(L=p,H.abort(),0!==I&&(clearTimeout(I),I=0),F>16*B&&(F=16*B),F>A&&(F=A),I=setTimeout(P,F),F=2*F+1,E.readyState=q,c=new e("error"),E.dispatchEvent(c),h(E,E.onerror,c))}function k(){j(!1)}function l(){j(!0)}b=String(b);var z=Boolean(m&&d&&d.withCredentials),B=g(d?d.retry:0/0,1e3),C=g(d?d.heartbeatTimeout:0/0,45e3),D=d&&d.lastEventId&&String(d.lastEventId)||"",E=this,F=B,G=!1,H=new o,I=0,J=0,K=0,L=p,M=[],N="",O="",P=null,Q=u,R="",S="";d=null,n&&(J=setTimeout(function T(){3===H.readyState&&k(),J=setTimeout(T,500)},0)),P=function(){if(I=0,L!==p)return void j(!1);if(n&&(void 0!==H.sendAsBinary||void 0===H.onloadend)&&a.document&&a.document.readyState&&"complete"!==a.document.readyState)return void(I=setTimeout(P,4));H.onload=H.onerror=l,n&&(H.onabort=l,H.onreadystatechange=k),H.onprogress=k,G=!1,I=setTimeout(P,C),K=0,L=q,M.length=0,O="",N=D,S="",R="",Q=u;var c=b.slice(0,5);c="data:"!==c&&"blob:"!==c?b+((-1===b.indexOf("?",0)?"?":"&")+"lastEventId="+encodeURIComponent(D)+"&r="+String(Math.random()+1).slice(2)):b,H.open("GET",c,!0),n&&(H.withCredentials=z,H.responseType="text",H.setRequestHeader("Accept","text/event-stream")),H.send(null)},c.call(this),this.close=i,this.url=b,this.readyState=q,this.withCredentials=z,this.onopen=null,this.onmessage=null,this.onerror=null,P()}function j(){this.CONNECTING=q,this.OPEN=r,this.CLOSED=s}b.prototype={get:function(a){return this.data[a+"~"]},set:function(a,b){this.data[a+"~"]=b},"delete":function(a){delete this.data[a+"~"]}},c.prototype={dispatchEvent:function(a){a.target=this;var b=String(a.type),c=this.listeners,e=c.get(b);if(e)for(var f=e.length,g=-1,h=null;++g<f;){h=e[g];try{h.call(this,a)}catch(i){d(i)}}},addEventListener:function(a,b){a=String(a);var c=this.listeners,d=c.get(a);d||(d=[],c.set(a,d));for(var e=d.length;--e>=0;)if(d[e]===b)return;d.push(b)},removeEventListener:function(a,b){a=String(a);var c=this.listeners,d=c.get(a);if(d){for(var e=d.length,f=[],g=-1;++g<e;)d[g]!==b&&f.push(d[g]);0===f.length?c["delete"](a):c.set(a,f)}}},f.prototype=e.prototype;var k=a.XMLHttpRequest,l=a.XDomainRequest,m=Boolean(k&&void 0!==(new k).withCredentials),n=m,o=m?k:l,p=-1,q=0,r=1,s=2,t=3,u=4,v=5,w=6,x=7,y=/^text\/event\-stream;?(\s*charset\=utf\-8)?$/i,z=1e3,A=18e6;j.prototype=c.prototype,i.prototype=new j,j.call(i),o&&(a.NativeEventSource=a.EventSource,a.EventSource=i)}(this);