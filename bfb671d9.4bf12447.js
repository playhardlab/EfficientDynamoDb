(window.webpackJsonp=window.webpackJsonp||[]).push([[36],{106:function(e,t,n){"use strict";n.r(t),n.d(t,"frontMatter",(function(){return o})),n.d(t,"metadata",(function(){return c})),n.d(t,"toc",(function(){return l})),n.d(t,"default",(function(){return p}));var r=n(3),a=n(7),i=(n(0),n(118)),o={id:"transact",title:"Transact",slug:"../dev-guide/high-level/transact"},c={unversionedId:"dev_guide/high_level/transact",id:"dev_guide/high_level/transact",isDocsHomePage:!1,title:"Transact",description:"DynamoDB provides two transact operations:",source:"@site/docs/dev_guide/high_level/transact.md",slug:"/dev_guide/dev-guide/high-level/transact",permalink:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/transact",editUrl:"https://github.com/alloczero/EfficientDynamoDb/edit/master/website/docs/dev_guide/high_level/transact.md",version:"current",sidebar:"someSidebar",previous:{title:"Batch",permalink:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/batch"},next:{title:"Custom Converters",permalink:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/converters"}},l=[{value:"TransactGetItems",id:"transactgetitems",children:[]},{value:"TransactWriteItems",id:"transactwriteitems",children:[]}],s={toc:l};function p(e){var t=e.components,n=Object(a.a)(e,["components"]);return Object(i.b)("wrapper",Object(r.a)({},s,n,{components:t,mdxType:"MDXLayout"}),Object(i.b)("p",null,"DynamoDB provides two transact operations:"),Object(i.b)("ul",null,Object(i.b)("li",{parentName:"ul"},Object(i.b)("inlineCode",{parentName:"li"},"TransactGetItems")," - Atomically read multiple items from one or more tables."),Object(i.b)("li",{parentName:"ul"},Object(i.b)("inlineCode",{parentName:"li"},"TransactWriteItems")," -  Atomically modify multiple items in one or more tables.")),Object(i.b)("p",null,"Both operations throw ",Object(i.b)("inlineCode",{parentName:"p"},"TransactionCanceledException")," when transaction is rejected.\nThe ",Object(i.b)("inlineCode",{parentName:"p"},"CancellationReasons")," property can be used to find out the reason behind the rejection."),Object(i.b)("h2",{id:"transactgetitems"},"TransactGetItems"),Object(i.b)("p",null,"Atomically retrieves up to 25 items from one or more tables within the same AWS account and Region."),Object(i.b)("p",null,"Each entity primary key is configured using ",Object(i.b)("inlineCode",{parentName:"p"},"Transact.GetItem")," factory method."),Object(i.b)("pre",null,Object(i.b)("code",{parentName:"pre",className:"language-csharp"},'var items = await context.TransactGet()\n    .WithItems(\n        Transact.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_1"),\n        Transact.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_2")\n    )\n    .ToListAsync<MixedEntity>()\n')),Object(i.b)("p",null,Object(i.b)("em",{parentName:"p"},"Entities of different type can be retrieved by using ",Object(i.b)("inlineCode",{parentName:"em"},"AsDocuments()")," method the same way as for other read operations.")),Object(i.b)("h2",{id:"transactwriteitems"},"TransactWriteItems"),Object(i.b)("p",null,"Atomically applies one of four operations per item within the same AWS account and Region (up to 25 operations):"),Object(i.b)("ul",null,Object(i.b)("li",{parentName:"ul"},Object(i.b)("inlineCode",{parentName:"li"},"Transact.PutItem")," - applies a ",Object(i.b)("a",{parentName:"li",href:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/write#putitem"},"PutItem")," operation."),Object(i.b)("li",{parentName:"ul"},Object(i.b)("inlineCode",{parentName:"li"},"Transact.UpdateItem")," - applies an ",Object(i.b)("a",{parentName:"li",href:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/write#update"},"UpdateItem")," operation."),Object(i.b)("li",{parentName:"ul"},Object(i.b)("inlineCode",{parentName:"li"},"Transact.DeleteItem")," - applies a ",Object(i.b)("a",{parentName:"li",href:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/write#delete"},"DeleteItem")," operation."),Object(i.b)("li",{parentName:"ul"},Object(i.b)("inlineCode",{parentName:"li"},"Transact.ConditionCheck")," - applies a condition to an item which is not modified by the transaction.")),Object(i.b)("pre",null,Object(i.b)("code",{parentName:"pre",className:"language-csharp"},'await context.TransactWrite()\n    .WithClientRequestToken(idempotencyKey)\n    .WithItems(\n        Transact.PutItem(new UserEmailEntity("test@test.com")),\n        Transact.ConditionCheck<UserEntity>()\n            .WithPrimaryKey("partitionKey", "sortKey")\n            .WithCondition(Condition<UserEntity>.On(x => x.Verified).EqualsTo(false))\n    )\n    .ExecuteAsync();\n')))}p.isMDXComponent=!0},118:function(e,t,n){"use strict";n.d(t,"a",(function(){return d})),n.d(t,"b",(function(){return b}));var r=n(0),a=n.n(r);function i(e,t,n){return t in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function o(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);t&&(r=r.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,r)}return n}function c(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?o(Object(n),!0).forEach((function(t){i(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):o(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function l(e,t){if(null==e)return{};var n,r,a=function(e,t){if(null==e)return{};var n,r,a={},i=Object.keys(e);for(r=0;r<i.length;r++)n=i[r],t.indexOf(n)>=0||(a[n]=e[n]);return a}(e,t);if(Object.getOwnPropertySymbols){var i=Object.getOwnPropertySymbols(e);for(r=0;r<i.length;r++)n=i[r],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(a[n]=e[n])}return a}var s=a.a.createContext({}),p=function(e){var t=a.a.useContext(s),n=t;return e&&(n="function"==typeof e?e(t):c(c({},t),e)),n},d=function(e){var t=p(e.components);return a.a.createElement(s.Provider,{value:t},e.children)},m={inlineCode:"code",wrapper:function(e){var t=e.children;return a.a.createElement(a.a.Fragment,{},t)}},u=a.a.forwardRef((function(e,t){var n=e.components,r=e.mdxType,i=e.originalType,o=e.parentName,s=l(e,["components","mdxType","originalType","parentName"]),d=p(n),u=r,b=d["".concat(o,".").concat(u)]||d[u]||m[u]||i;return n?a.a.createElement(b,c(c({ref:t},s),{},{components:n})):a.a.createElement(b,c({ref:t},s))}));function b(e,t){var n=arguments,r=t&&t.mdxType;if("string"==typeof e||r){var i=n.length,o=new Array(i);o[0]=u;var c={};for(var l in t)hasOwnProperty.call(t,l)&&(c[l]=t[l]);c.originalType=e,c.mdxType="string"==typeof e?e:r,o[1]=c;for(var s=2;s<i;s++)o[s]=n[s];return a.a.createElement.apply(null,o)}return a.a.createElement.apply(null,n)}u.displayName="MDXCreateElement"}}]);