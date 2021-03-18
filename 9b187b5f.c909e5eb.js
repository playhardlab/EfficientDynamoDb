(window.webpackJsonp=window.webpackJsonp||[]).push([[25],{117:function(e,t,n){"use strict";n.d(t,"a",(function(){return b})),n.d(t,"b",(function(){return m}));var a=n(0),i=n.n(a);function r(e,t,n){return t in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function o(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);t&&(a=a.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,a)}return n}function c(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?o(Object(n),!0).forEach((function(t){r(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):o(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function s(e,t){if(null==e)return{};var n,a,i=function(e,t){if(null==e)return{};var n,a,i={},r=Object.keys(e);for(a=0;a<r.length;a++)n=r[a],t.indexOf(n)>=0||(i[n]=e[n]);return i}(e,t);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);for(a=0;a<r.length;a++)n=r[a],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(i[n]=e[n])}return i}var l=i.a.createContext({}),p=function(e){var t=i.a.useContext(l),n=t;return e&&(n="function"==typeof e?e(t):c(c({},t),e)),n},b=function(e){var t=p(e.components);return i.a.createElement(l.Provider,{value:t},e.children)},u={inlineCode:"code",wrapper:function(e){var t=e.children;return i.a.createElement(i.a.Fragment,{},t)}},d=i.a.forwardRef((function(e,t){var n=e.components,a=e.mdxType,r=e.originalType,o=e.parentName,l=s(e,["components","mdxType","originalType","parentName"]),b=p(n),d=a,m=b["".concat(o,".").concat(d)]||b[d]||u[d]||r;return n?i.a.createElement(m,c(c({ref:t},l),{},{components:n})):i.a.createElement(m,c({ref:t},l))}));function m(e,t){var n=arguments,a=t&&t.mdxType;if("string"==typeof e||a){var r=n.length,o=new Array(r);o[0]=d;var c={};for(var s in t)hasOwnProperty.call(t,s)&&(c[s]=t[s]);c.originalType=e,c.mdxType="string"==typeof e?e:a,o[1]=c;for(var l=2;l<r;l++)o[l]=n[l];return i.a.createElement.apply(null,o)}return i.a.createElement.apply(null,n)}d.displayName="MDXCreateElement"},96:function(e,t,n){"use strict";n.r(t),n.d(t,"frontMatter",(function(){return o})),n.d(t,"metadata",(function(){return c})),n.d(t,"toc",(function(){return s})),n.d(t,"default",(function(){return p}));var a=n(3),i=n(7),r=(n(0),n(117)),o={id:"read",title:"Reading Data",slug:"../dev-guide/high-level/read"},c={unversionedId:"dev_guide/high_level/read",id:"dev_guide/high_level/read",isDocsHomePage:!1,title:"Reading Data",description:"DynamoDB provides three main operations for reads:",source:"@site/docs/dev_guide/high_level/read.md",slug:"/dev_guide/dev-guide/high-level/read",permalink:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/read",editUrl:"https://github.com/alloczero/EfficientDynamoDb/edit/master/website/docs/dev_guide/high_level/read.md",version:"current",sidebar:"someSidebar",previous:{title:"Attributes",permalink:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/attributes"},next:{title:"Writing Data",permalink:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/write"}},s=[{value:"Retrieving an item",id:"retrieving-an-item",children:[]},{value:"Querying data",id:"querying-data",children:[]},{value:"Scanning data",id:"scanning-data",children:[{value:"Parallel Scan",id:"parallel-scan",children:[]}]},{value:"Document returns",id:"document-returns",children:[]},{value:"Projections",id:"projections",children:[]},{value:"Pagination",id:"pagination",children:[]},{value:"Useful links",id:"useful-links",children:[]}],l={toc:s};function p(e){var t=e.components,n=Object(i.a)(e,["components"]);return Object(r.b)("wrapper",Object(a.a)({},l,n,{components:t,mdxType:"MDXLayout"}),Object(r.b)("p",null,"DynamoDB provides three main operations for reads:"),Object(r.b)("ul",null,Object(r.b)("li",{parentName:"ul"},Object(r.b)("inlineCode",{parentName:"li"},"GetItem")," - Read a single item."),Object(r.b)("li",{parentName:"ul"},Object(r.b)("inlineCode",{parentName:"li"},"Query")," - Get items from a single partition based on provided query expression."),Object(r.b)("li",{parentName:"ul"},Object(r.b)("inlineCode",{parentName:"li"},"Scan")," - Get all items from a table with a possibility to filter some of the results out.")),Object(r.b)("p",null,"DynamoDB also supports a ",Object(r.b)("inlineCode",{parentName:"p"},"BatchGetItem")," operation for executing up to 100 ",Object(r.b)("inlineCode",{parentName:"p"},"GetItem")," operations in a single request.\nIt's covered in ",Object(r.b)("a",{parentName:"p",href:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/batch"},"batch operations guide"),"."),Object(r.b)("h2",{id:"retrieving-an-item"},"Retrieving an item"),Object(r.b)("p",null,"To read an item from a DynamoDB table, use the ",Object(r.b)("inlineCode",{parentName:"p"},"GetItem")," operation.\nYou must provide a type marked by ",Object(r.b)("a",{parentName:"p",href:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/attributes#dynamodbtable"},"DynamoDBTable")," attribute."),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},'var item = await ddbContext.GetItemAsync<EntityClass>("partitionKey");\n')),Object(r.b)("p",null,"You must specify the ",Object(r.b)("em",{parentName:"p"},"entire")," primary key, not just part of it.\nFor example, if a table has a composite primary key (partition key and sort key), you must supply a value for the partition key and a value for the sort key."),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},'var item = await ddbContext.GetItemAsync<EntityClass>("partitionKey", "sortKey");\n')),Object(r.b)("p",null,"You can use the fluent API when you need better control over the operation behavior."),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},'var item = await ddbContext.GetItem<EntityClass>()\n    .WithConsistentRead(true)\n    .WithPrimaryKey("partitionKey", "sortKey")\n    .ToItemAsync();\n')),Object(r.b)("h2",{id:"querying-data"},"Querying data"),Object(r.b)("p",null,"The ",Object(r.b)("inlineCode",{parentName:"p"},"Query")," operation in Amazon DynamoDB finds items based on primary key values."),Object(r.b)("p",null,"Since ",Object(r.b)("inlineCode",{parentName:"p"},"Query")," is a rather complicated operation, you can only use fluent API to perform it.\nYou must provide the ",Object(r.b)("inlineCode",{parentName:"p"},"KeyExpression")," in every request."),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},'var condition = Condition<EntityClass>.On(x => x.Pk).EqualsTo("test");\n\nvar items = await ddbContext.Query<EntityClass>()\n    .WithKeyExpression(condition)\n    .ToListAsync();\n')),Object(r.b)("p",null,"DynamoDB can only return up to 1 MB of data per response.\nIf your query contains more, DynamoDB will paginate the response.\nIn this case, ",Object(r.b)("inlineCode",{parentName:"p"},"ToListAsync()")," makes multiple calls until all the data is fetched and put into a single resulting array."),Object(r.b)("p",null,"Check the ",Object(r.b)("a",{parentName:"p",href:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/conditions"},"condition building guide")," for detailed information about condition builder API."),Object(r.b)("h2",{id:"scanning-data"},"Scanning data"),Object(r.b)("p",null,"The ",Object(r.b)("inlineCode",{parentName:"p"},"Scan")," operation iterates over the whole table and returns values that satisfy ",Object(r.b)("inlineCode",{parentName:"p"},"FilterExpression")," if set.\nFluent API is the only option for high-level scanning."),Object(r.b)("p",null,"Unlike the ",Object(r.b)("inlineCode",{parentName:"p"},"Query"),", ",Object(r.b)("inlineCode",{parentName:"p"},"Scan")," API doesn't have a ",Object(r.b)("inlineCode",{parentName:"p"},"ToListAsync()")," method to encourage better table design for your DB and correct scanning usage.\nThe closest replacement is ",Object(r.b)("inlineCode",{parentName:"p"},"ToAsyncEnumerable()")),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},"var scan = ddbContext.Scan<EntityClass>();\n\nawait foreach (var item in scan.ToAsyncEnumerable())\n{\n    // Process an item here.\n}\n")),Object(r.b)("h3",{id:"parallel-scan"},"Parallel Scan"),Object(r.b)("p",null,"DynamoDB supports parallel scans that are straightforward to use with EfficientDynamoDb.\nAll you need to do is decide the number of scanning segments and pass it in the ",Object(r.b)("inlineCode",{parentName:"p"},"ToParallelAsyncEnumerable(...)")," method."),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},"var scan = ddbContext.Scan<EntityClass>();\nvar segmentsCount = 8;\n\nawait foreach (var item in scan.ToParallelAsyncEnumerable(segmentsCount))\n{\n    // Process an item here.\n}\n")),Object(r.b)("h2",{id:"document-returns"},"Document returns"),Object(r.b)("p",null,"Sometimes, your queries return different entities in a single response.\nIt frequently happens when you utilize a single-table design."),Object(r.b)("p",null,"Fluent API allows you to return ",Object(r.b)("inlineCode",{parentName:"p"},"Document")," objects instead of your entities which you can convert to correct entities in applications code.\nJust call the ",Object(r.b)("inlineCode",{parentName:"p"},"AsDocument()")," (for ",Object(r.b)("inlineCode",{parentName:"p"},"GetItem"),") or ",Object(r.b)("inlineCode",{parentName:"p"},"AsDocuments()")," (for ",Object(r.b)("inlineCode",{parentName:"p"},"Query")," and ",Object(r.b)("inlineCode",{parentName:"p"},"Scan"),") anywhere in the call chain before the executing method\n(e.g., ",Object(r.b)("inlineCode",{parentName:"p"},"ToItemAsync()")," for ",Object(r.b)("inlineCode",{parentName:"p"},"GetItem"),", ",Object(r.b)("inlineCode",{parentName:"p"},"ToListAsync()")," for ",Object(r.b)("inlineCode",{parentName:"p"},"Query"),", etc.)"),Object(r.b)("p",null,"For example, consider the case when a single query returns the user's profile data and a list of his transactions."),Object(r.b)("p",null,"Retrieving documents using the ",Object(r.b)("inlineCode",{parentName:"p"},"Query")," operation:"),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},'var condition = Condition<EntityClass>.On(x => x.Pk).EqualsTo("test");\n\nvar documents = await ddbContext.Query<EntityClass>()\n    .WithKeyExpression(condition)\n    .AsDocuments()\n    .ToListAsync();\n')),Object(r.b)("p",null,"Mapping documents to entities can be done by calling the ",Object(r.b)("inlineCode",{parentName:"p"},"Document.ToObject<T>()")," method:"),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},'// sort key prefix determines the \'type\' of item\nvar userInfoDocument = documents.First(x => x["sortKey"].StartsWith("userInfo#"));\n\n// convert Document to entity class\nvar userInfo = ddbContext.ToObject<UserInfo>(userInfoDocument); \n\n// assuming that all other items except user info are transactions\nvar transactions = documents.Except(userInfoDocument) \n    .Select(x => ddbContext.ToObject<UserTransaction>(x))\n    .ToList();\n')),Object(r.b)("p",null,Object(r.b)("inlineCode",{parentName:"p"},"GetItem")," document example:"),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},'var item = await ddbContext.GetItem<EntityClass>()\n    .AsDocument()\n    .WithPrimaryKey("partitionKey", "sortKey")\n    .ToItemAsync();\n')),Object(r.b)("p",null,Object(r.b)("inlineCode",{parentName:"p"},"Scan")," example:"),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},"var scan = ddbContext.Scan<EntityClass>().AsDocuments();\n\nawait foreach (var item in scan.ToAsyncEnumerable())\n{\n    // Process an item here.\n}\n")),Object(r.b)("h2",{id:"projections"},"Projections"),Object(r.b)("p",null,"Use projections to retrieve only specific attributes of item(s).\nAll read operations support projection using the same API set."),Object(r.b)("p",null,"Use the ",Object(r.b)("inlineCode",{parentName:"p"},"AsProjection<TProjection>()")," method to get a projection to the specified class."),Object(r.b)("p",null,Object(r.b)("strong",{parentName:"p"},"Projected class and its properties must be marked with corresponding attributes in the same way as entities are marked!")),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},'var projectedItem = await ddbContext.GetItem<EntityClass>()\n    .AsProjection<ProjectionClass>()\n    .WithPrimaryKey("partitionKey", "sortKey")\n    .ToItemAsync()\n')),Object(r.b)("p",null,"Use the ",Object(r.b)("inlineCode",{parentName:"p"},"WithProjectedAttributes(...)")," method if you don't want to create a separate projection class.\nWhen this method is used, the response will keep the original entity class but pull and populate only specified attributes."),Object(r.b)("p",null,Object(r.b)("strong",{parentName:"p"},"Passing the same property multiple times is not allowed!")),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},'var item = await ddbContext.GetItem<EntityClass>()\n    .WithProjectedAttributes(x => x.FirstName, x => x.LastName)\n    .WithPrimaryKey("partitionKey", "sortKey")\n    .ToItemAsync()\n')),Object(r.b)("h2",{id:"pagination"},"Pagination"),Object(r.b)("p",null,Object(r.b)("inlineCode",{parentName:"p"},"Scan")," and ",Object(r.b)("inlineCode",{parentName:"p"},"Query")," have two ways of handling paginated requests.\nAPIs for both operations are the same, so that the following examples will show only ",Object(r.b)("inlineCode",{parentName:"p"},"Query")," for the sake of simplicity."),Object(r.b)("p",null,"The easiest way to handle a paginated request manually is to use ",Object(r.b)("inlineCode",{parentName:"p"},"ToAsyncEnumerable()"),"."),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},"await foreach (var item in query.ToAsyncEnumerable())\n{\n    // Process an item here.\n}\n")),Object(r.b)("p",null,"There are also cases when you might need to manage pagination tokens yourself.\nTo do so, use the ",Object(r.b)("inlineCode",{parentName:"p"},"ToPageAsync()")," to get the pagination token in response and then pass it to the subsequent request."),Object(r.b)("pre",null,Object(r.b)("code",{parentName:"pre",className:"language-csharp"},"var page = await query.ToPageAsync();\n\nvar nextPage = await query.WithPaginationToken(page.PaginationToken).ToPageAsync();\n")),Object(r.b)("p",null,"Note: ",Object(r.b)("em",{parentName:"p"},"Due to the internals of the DynamoDB, ",Object(r.b)("inlineCode",{parentName:"em"},"page.Items")," being empty doesn't mean that there are no more data to read."),"\n",Object(r.b)("em",{parentName:"p"},"The only way to know that all data is retrieved is by checking the ",Object(r.b)("inlineCode",{parentName:"em"},"page.PaginationToken")," value. It is ",Object(r.b)("inlineCode",{parentName:"em"},"null")," when there are no more items to pull"),"."),Object(r.b)("h2",{id:"useful-links"},"Useful links"),Object(r.b)("ul",null,Object(r.b)("li",{parentName:"ul"},"API references",Object(r.b)("ul",{parentName:"li"},Object(r.b)("li",{parentName:"ul"},Object(r.b)("a",{parentName:"li",href:"/EfficientDynamoDb/docs/api-reference/get-item"},"GetItem")),Object(r.b)("li",{parentName:"ul"},Object(r.b)("a",{parentName:"li",href:"/EfficientDynamoDb/docs/api-reference/query"},"Query")),Object(r.b)("li",{parentName:"ul"},Object(r.b)("a",{parentName:"li",href:"/EfficientDynamoDb/docs/api-reference/scan"},"Scan")))),Object(r.b)("li",{parentName:"ul"},Object(r.b)("a",{parentName:"li",href:"/EfficientDynamoDb/docs/dev_guide/dev-guide/high-level/conditions"},"Condition Builder guide"))))}p.isMDXComponent=!0}}]);