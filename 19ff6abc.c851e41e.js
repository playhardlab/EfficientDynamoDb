(window.webpackJsonp=window.webpackJsonp||[]).push([[7],{115:function(e,t,n){"use strict";n.d(t,"a",(function(){return p})),n.d(t,"b",(function(){return d}));var r=n(0),i=n.n(r);function a(e,t,n){return t in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function o(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);t&&(r=r.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,r)}return n}function c(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?o(Object(n),!0).forEach((function(t){a(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):o(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function l(e,t){if(null==e)return{};var n,r,i=function(e,t){if(null==e)return{};var n,r,i={},a=Object.keys(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||(i[n]=e[n]);return i}(e,t);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(i[n]=e[n])}return i}var s=i.a.createContext({}),u=function(e){var t=i.a.useContext(s),n=t;return e&&(n="function"==typeof e?e(t):c(c({},t),e)),n},p=function(e){var t=u(e.components);return i.a.createElement(s.Provider,{value:t},e.children)},b={inlineCode:"code",wrapper:function(e){var t=e.children;return i.a.createElement(i.a.Fragment,{},t)}},m=i.a.forwardRef((function(e,t){var n=e.components,r=e.mdxType,a=e.originalType,o=e.parentName,s=l(e,["components","mdxType","originalType","parentName"]),p=u(n),m=r,d=p["".concat(o,".").concat(m)]||p[m]||b[m]||a;return n?i.a.createElement(d,c(c({ref:t},s),{},{components:n})):i.a.createElement(d,c({ref:t},s))}));function d(e,t){var n=arguments,r=t&&t.mdxType;if("string"==typeof e||r){var a=n.length,o=new Array(a);o[0]=m;var c={};for(var l in t)hasOwnProperty.call(t,l)&&(c[l]=t[l]);c.originalType=e,c.mdxType="string"==typeof e?e:r,o[1]=c;for(var s=2;s<a;s++)o[s]=n[s];return i.a.createElement.apply(null,o)}return i.a.createElement.apply(null,n)}m.displayName="MDXCreateElement"},75:function(e,t,n){"use strict";n.r(t),n.d(t,"frontMatter",(function(){return o})),n.d(t,"metadata",(function(){return c})),n.d(t,"toc",(function(){return l})),n.d(t,"default",(function(){return u}));var r=n(3),i=n(7),a=(n(0),n(115)),o={id:"batch",title:"Batch Operations",slug:"../dev-guide/high-level/batch"},c={unversionedId:"dev_guide/high_level/batch",id:"dev_guide/high_level/batch",isDocsHomePage:!1,title:"Batch Operations",description:"DynamoDB provides two batch operations:",source:"@site/docs/dev_guide/high_level/batch.md",slug:"/dev_guide/dev-guide/high-level/batch",permalink:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/batch",editUrl:"https://github.com/alloczero/EfficientDynamoDb/edit/master/website/docs/dev_guide/high_level/batch.md",version:"current",sidebar:"someSidebar",previous:{title:"Writing Data",permalink:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/write"},next:{title:"Transactions",permalink:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/transact"}},l=[{value:"BatchGetItem",id:"batchgetitem",children:[]},{value:"BatchWriteItem",id:"batchwriteitem",children:[]}],s={toc:l};function u(e){var t=e.components,n=Object(i.a)(e,["components"]);return Object(a.b)("wrapper",Object(r.a)({},s,n,{components:t,mdxType:"MDXLayout"}),Object(a.b)("p",null,"DynamoDB provides two batch operations:"),Object(a.b)("ul",null,Object(a.b)("li",{parentName:"ul"},Object(a.b)("inlineCode",{parentName:"li"},"BatchGetItem")," - Read multiple items from one or more tables."),Object(a.b)("li",{parentName:"ul"},Object(a.b)("inlineCode",{parentName:"li"},"BatchWriteItem")," -  Put or delete multiple items in one or more tables.")),Object(a.b)("p",null,"EfficientDynamoDb automatically delays and retries in case if batch operation returned unprocessed items,\nwhich can happen when provisioned throughput is exceeded, size limit reached, or internal DynamoDB error occurred."),Object(a.b)("h2",{id:"batchgetitem"},"BatchGetItem"),Object(a.b)("p",null,"Reads up to 100 items in a single request."),Object(a.b)("p",null,"Each entity's primary key is configured using ",Object(a.b)("inlineCode",{parentName:"p"},"Batch.GetItem")," factory method."),Object(a.b)("pre",null,Object(a.b)("code",{parentName:"pre",className:"language-csharp"},' var items = await context.BatchGet()\n     .WithItems(\n         Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_1"),\n         Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_2")\n     )\n     .ToListAsync<EntityClass>();\n')),Object(a.b)("p",null,"When a strong consistency or a projection is needed, a more sophisticated ",Object(a.b)("inlineCode",{parentName:"p"},"FromTables")," method can be used:"),Object(a.b)("pre",null,Object(a.b)("code",{parentName:"pre",className:"language-csharp"},' var items = await context.BatchGet()\n     .FromTables(\n        Batch.FromTable<EntityClass>()\n            .WithConsistentRead(true)\n            .WithProjectedAttributes<ProjectionClass>()\n            .WithItems(\n                Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_1"),\n                Batch.GetItem<EntityClass>().WithPrimaryKey("partitionKey", "sortKey_2")\n                )\n    )\n     .ToListAsync<EntityClass>();\n')),Object(a.b)("p",null,Object(a.b)("em",{parentName:"p"},"Entities of different type can be retrieved by using ",Object(a.b)("inlineCode",{parentName:"em"},"AsDocuments()")," method the same way as for other read operations.")),Object(a.b)("h2",{id:"batchwriteitem"},"BatchWriteItem"),Object(a.b)("p",null,"Puts or deletes up to 25 items in a single request."),Object(a.b)("p",null,"Each write operation is configured using either ",Object(a.b)("inlineCode",{parentName:"p"},"Batch.PutItem")," or ",Object(a.b)("inlineCode",{parentName:"p"},"Batch.DeleteItem")," factory method."),Object(a.b)("pre",null,Object(a.b)("code",{parentName:"pre",className:"language-csharp"},'await context.BatchWrite()\n    .WithItems(\n        Batch.PutItem(new UserEntity("John", "Doe")),\n        Batch.DeleteItem<UserEntity>().WithPrimaryKey("partitionKey", "sortKey")\n    )\n    .ExecuteAsync();\n')))}u.isMDXComponent=!0}}]);