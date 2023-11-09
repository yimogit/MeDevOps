## 安装

docker compose up -d

## 浏览器直接打开连接 JumpServer WebSite

搭配 tampermonkey 自定义脚本，屏蔽点击事件，并直接打开网站

```
// ==UserScript==
// @name         直接打开 JumpServer WebSite
// @namespace    http://tampermonkey.net/
// @version      0.1
// @description  try to take over the world!
// @author       yimo
// @match        https://jumpserver.devops.test.com/luna/?_=1699534259406
// @icon         https://www.google.com/s2/favicons?sz=64&domain=test.com
// @grant        none
// ==/UserScript==

(function() {
    'use strict';

    window.onload=function(){
        console.log('load custom js')
        setInterval(()=>{
            document.querySelectorAll(".node_name").forEach(s=>{
                if(s.isAddHtml)return;
                s.isAddHtml=true;
                if(s.parentNode.getAttribute("title").indexOf('http')==0){
                    console.log('检测到站点:'+s.parentNode.getAttribute("title"))
                    s.addEventListener('click',function(e){
                         console.log('click')
                         var titleUrl = s.parentNode.getAttribute("title");
                         window.open(titleUrl);
                         e.stopPropagation();
                    })
                    return;
                }
//                 s.addEventListener('click',function(){
//                     setInterval(()=>{
//                         document.querySelectorAll("a .website_ico_docu").forEach(node => {
//                             if(node.parentNode.isAddHtml)return;
//                             var titleUrl = node.parentNode.getAttribute("title");
//                             if(titleUrl.indexOf('http')!=0)return;
//                             node.parentNode.isAddHtml=true
//                             var aTag = document.createElement('a');
//                             aTag.innerHTML = '打开';
//                             aTag.href = titleUrl;
//                             aTag.style="margin-right:5px;color: #1ab394;";
//                             aTag.target="_blank";
//                             node.parentNode.insertBefore(aTag, node);
//                         });
//                     },500)

//                 },false)
            })
        },500);
    }
    // Your code here...
})();
```
