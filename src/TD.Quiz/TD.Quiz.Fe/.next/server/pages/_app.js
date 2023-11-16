/*
 * ATTENTION: An "eval-source-map" devtool has been used.
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file with attached SourceMaps in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
(() => {
var exports = {};
exports.id = "pages/_app";
exports.ids = ["pages/_app"];
exports.modules = {

/***/ "./src/app/store.ts":
/*!**************************!*\
  !*** ./src/app/store.ts ***!
  \**************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   selectCorrectAnswersCount: () => (/* binding */ selectCorrectAnswersCount),\n/* harmony export */   selectIncorrectAnswersCount: () => (/* binding */ selectIncorrectAnswersCount),\n/* harmony export */   store: () => (/* binding */ store)\n/* harmony export */ });\n/* harmony import */ var _features_userAnswers_userAnswersSlice__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @/features/userAnswers/userAnswersSlice */ \"./src/features/userAnswers/userAnswersSlice.ts\");\n/* harmony import */ var _reduxjs_toolkit__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @reduxjs/toolkit */ \"@reduxjs/toolkit\");\n/* harmony import */ var _reduxjs_toolkit__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_reduxjs_toolkit__WEBPACK_IMPORTED_MODULE_1__);\n\n\nconst store = (0,_reduxjs_toolkit__WEBPACK_IMPORTED_MODULE_1__.configureStore)({\n    reducer: {\n        userAnswers: _features_userAnswers_userAnswersSlice__WEBPACK_IMPORTED_MODULE_0__[\"default\"]\n    }\n});\nconst selectCorrectAnswersCount = (state)=>state.userAnswers.correctAnswers;\nconst selectIncorrectAnswersCount = (state)=>state.userAnswers.incorrectAnswers;\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9zcmMvYXBwL3N0b3JlLnRzIiwibWFwcGluZ3MiOiI7Ozs7Ozs7OztBQUF1RTtBQUNyQjtBQUUzQyxNQUFNRSxRQUFRRCxnRUFBY0EsQ0FBQztJQUNoQ0UsU0FBUztRQUNMQyxhQUFhSiw4RUFBZ0JBO0lBQ2pDO0FBQ0osR0FBRTtBQUlLLE1BQU1LLDRCQUE0QixDQUFDQyxRQUFxQkEsTUFBTUYsV0FBVyxDQUFDRyxjQUFjO0FBQ3hGLE1BQU1DLDhCQUE4QixDQUFDRixRQUFxQkEsTUFBTUYsV0FBVyxDQUFDSyxnQkFBZ0IiLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly90ZC1xdWl6ei1mZS8uL3NyYy9hcHAvc3RvcmUudHM/MTQ3YiJdLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgdXNlckFuc3dlcnNTbGljZSBmcm9tIFwiQC9mZWF0dXJlcy91c2VyQW5zd2Vycy91c2VyQW5zd2Vyc1NsaWNlXCI7XHJcbmltcG9ydCB7IGNvbmZpZ3VyZVN0b3JlIH0gZnJvbSBcIkByZWR1eGpzL3Rvb2xraXRcIjtcclxuXHJcbmV4cG9ydCBjb25zdCBzdG9yZSA9IGNvbmZpZ3VyZVN0b3JlKHtcclxuICAgIHJlZHVjZXI6IHtcclxuICAgICAgICB1c2VyQW5zd2VyczogdXNlckFuc3dlcnNTbGljZVxyXG4gICAgfVxyXG59KVxyXG5cclxuZXhwb3J0IHR5cGUgUm9vdFN0YXRlID0gUmV0dXJuVHlwZTx0eXBlb2Ygc3RvcmUuZ2V0U3RhdGU+XHJcblxyXG5leHBvcnQgY29uc3Qgc2VsZWN0Q29ycmVjdEFuc3dlcnNDb3VudCA9IChzdGF0ZTogUm9vdFN0YXRlKSA9PiBzdGF0ZS51c2VyQW5zd2Vycy5jb3JyZWN0QW5zd2Vyc1xyXG5leHBvcnQgY29uc3Qgc2VsZWN0SW5jb3JyZWN0QW5zd2Vyc0NvdW50ID0gKHN0YXRlOiBSb290U3RhdGUpID0+IHN0YXRlLnVzZXJBbnN3ZXJzLmluY29ycmVjdEFuc3dlcnNcclxuXHJcbmV4cG9ydCB0eXBlIEFwcERpc3BhdGNoID0gdHlwZW9mIHN0b3JlLmRpc3BhdGNoIl0sIm5hbWVzIjpbInVzZXJBbnN3ZXJzU2xpY2UiLCJjb25maWd1cmVTdG9yZSIsInN0b3JlIiwicmVkdWNlciIsInVzZXJBbnN3ZXJzIiwic2VsZWN0Q29ycmVjdEFuc3dlcnNDb3VudCIsInN0YXRlIiwiY29ycmVjdEFuc3dlcnMiLCJzZWxlY3RJbmNvcnJlY3RBbnN3ZXJzQ291bnQiLCJpbmNvcnJlY3RBbnN3ZXJzIl0sInNvdXJjZVJvb3QiOiIifQ==\n//# sourceURL=webpack-internal:///./src/app/store.ts\n");

/***/ }),

/***/ "./src/features/userAnswers/userAnswersSlice.ts":
/*!******************************************************!*\
  !*** ./src/features/userAnswers/userAnswersSlice.ts ***!
  \******************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (__WEBPACK_DEFAULT_EXPORT__),\n/* harmony export */   increaseCorrectAnswers: () => (/* binding */ increaseCorrectAnswers),\n/* harmony export */   increaseIncorrectAnswers: () => (/* binding */ increaseIncorrectAnswers),\n/* harmony export */   userAnswersSlice: () => (/* binding */ userAnswersSlice)\n/* harmony export */ });\n/* harmony import */ var _reduxjs_toolkit__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @reduxjs/toolkit */ \"@reduxjs/toolkit\");\n/* harmony import */ var _reduxjs_toolkit__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_reduxjs_toolkit__WEBPACK_IMPORTED_MODULE_0__);\n\nconst initialState = {\n    correctAnswers: 0,\n    incorrectAnswers: 0\n};\nconst userAnswersSlice = (0,_reduxjs_toolkit__WEBPACK_IMPORTED_MODULE_0__.createSlice)({\n    name: \"usersAnswers\",\n    initialState,\n    reducers: {\n        increaseCorrectAnswers: (state, action)=>{\n            state.correctAnswers += action.payload;\n        },\n        increaseIncorrectAnswers: (state, action)=>{\n            state.incorrectAnswers += action.payload;\n        }\n    }\n});\nconst { increaseCorrectAnswers, increaseIncorrectAnswers } = userAnswersSlice.actions;\n/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (userAnswersSlice.reducer);\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9zcmMvZmVhdHVyZXMvdXNlckFuc3dlcnMvdXNlckFuc3dlcnNTbGljZS50cyIsIm1hcHBpbmdzIjoiOzs7Ozs7Ozs7QUFBOEQ7QUFPOUQsTUFBTUMsZUFBaUM7SUFDbkNDLGdCQUFnQjtJQUNoQkMsa0JBQWtCO0FBQ3RCO0FBRU8sTUFBTUMsbUJBQW1CSiw2REFBV0EsQ0FBQztJQUN4Q0ssTUFBTTtJQUNOSjtJQUNBSyxVQUFVO1FBQ05DLHdCQUF3QixDQUFDQyxPQUFPQztZQUM1QkQsTUFBTU4sY0FBYyxJQUFJTyxPQUFPQyxPQUFPO1FBQzFDO1FBQ0FDLDBCQUEwQixDQUFDSCxPQUFPQztZQUM5QkQsTUFBTUwsZ0JBQWdCLElBQUlNLE9BQU9DLE9BQU87UUFDNUM7SUFDSjtBQUNKLEdBQUU7QUFFSyxNQUFNLEVBQUVILHNCQUFzQixFQUFFSSx3QkFBd0IsRUFBRSxHQUFHUCxpQkFBaUJRLE9BQU87QUFFNUYsaUVBQWVSLGlCQUFpQlMsT0FBTyIsInNvdXJjZXMiOlsid2VicGFjazovL3RkLXF1aXp6LWZlLy4vc3JjL2ZlYXR1cmVzL3VzZXJBbnN3ZXJzL3VzZXJBbnN3ZXJzU2xpY2UudHM/ZmE4ZiJdLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBQYXlsb2FkQWN0aW9uLCBjcmVhdGVTbGljZSB9IGZyb20gXCJAcmVkdXhqcy90b29sa2l0XCI7XHJcblxyXG5pbnRlcmZhY2UgVXNlckFuc3dlcnNTdGF0ZSB7XHJcbiAgICBjb3JyZWN0QW5zd2VyczogbnVtYmVyLFxyXG4gICAgaW5jb3JyZWN0QW5zd2VyczogbnVtYmVyXHJcbn1cclxuXHJcbmNvbnN0IGluaXRpYWxTdGF0ZTogVXNlckFuc3dlcnNTdGF0ZSA9IHtcclxuICAgIGNvcnJlY3RBbnN3ZXJzOiAwLFxyXG4gICAgaW5jb3JyZWN0QW5zd2VyczogMFxyXG59XHJcblxyXG5leHBvcnQgY29uc3QgdXNlckFuc3dlcnNTbGljZSA9IGNyZWF0ZVNsaWNlKHtcclxuICAgIG5hbWU6ICd1c2Vyc0Fuc3dlcnMnLFxyXG4gICAgaW5pdGlhbFN0YXRlLFxyXG4gICAgcmVkdWNlcnM6IHtcclxuICAgICAgICBpbmNyZWFzZUNvcnJlY3RBbnN3ZXJzOiAoc3RhdGUsIGFjdGlvbjogUGF5bG9hZEFjdGlvbjxudW1iZXI+KSA9PiB7XHJcbiAgICAgICAgICAgIHN0YXRlLmNvcnJlY3RBbnN3ZXJzICs9IGFjdGlvbi5wYXlsb2FkXHJcbiAgICAgICAgfSxcclxuICAgICAgICBpbmNyZWFzZUluY29ycmVjdEFuc3dlcnM6IChzdGF0ZSwgYWN0aW9uOiBQYXlsb2FkQWN0aW9uPG51bWJlcj4pID0+IHtcclxuICAgICAgICAgICAgc3RhdGUuaW5jb3JyZWN0QW5zd2VycyArPSBhY3Rpb24ucGF5bG9hZFxyXG4gICAgICAgIH1cclxuICAgIH1cclxufSlcclxuXHJcbmV4cG9ydCBjb25zdCB7IGluY3JlYXNlQ29ycmVjdEFuc3dlcnMsIGluY3JlYXNlSW5jb3JyZWN0QW5zd2VycyB9ID0gdXNlckFuc3dlcnNTbGljZS5hY3Rpb25zXHJcblxyXG5leHBvcnQgZGVmYXVsdCB1c2VyQW5zd2Vyc1NsaWNlLnJlZHVjZXIiXSwibmFtZXMiOlsiY3JlYXRlU2xpY2UiLCJpbml0aWFsU3RhdGUiLCJjb3JyZWN0QW5zd2VycyIsImluY29ycmVjdEFuc3dlcnMiLCJ1c2VyQW5zd2Vyc1NsaWNlIiwibmFtZSIsInJlZHVjZXJzIiwiaW5jcmVhc2VDb3JyZWN0QW5zd2VycyIsInN0YXRlIiwiYWN0aW9uIiwicGF5bG9hZCIsImluY3JlYXNlSW5jb3JyZWN0QW5zd2VycyIsImFjdGlvbnMiLCJyZWR1Y2VyIl0sInNvdXJjZVJvb3QiOiIifQ==\n//# sourceURL=webpack-internal:///./src/features/userAnswers/userAnswersSlice.ts\n");

/***/ }),

/***/ "./src/pages/_app.tsx":
/*!****************************!*\
  !*** ./src/pages/_app.tsx ***!
  \****************************/
/***/ ((module, __webpack_exports__, __webpack_require__) => {

"use strict";
eval("__webpack_require__.a(module, async (__webpack_handle_async_dependencies__, __webpack_async_result__) => { try {\n__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": () => (/* binding */ MyApp)\n/* harmony export */ });\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react/jsx-dev-runtime */ \"react/jsx-dev-runtime\");\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var _widgets_Layout__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @/widgets/Layout */ \"./src/widgets/Layout/index.ts\");\n/* harmony import */ var _barrel_optimize_names_ThemeProvider_mui_material__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! __barrel_optimize__?names=ThemeProvider!=!@mui/material */ \"./node_modules/@mui/material/styles/ThemeProvider.js\");\n/* harmony import */ var react_toastify__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! react-toastify */ \"react-toastify\");\n/* harmony import */ var _app_globals_css__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./../app/globals.css */ \"./src/app/globals.css\");\n/* harmony import */ var _app_globals_css__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_app_globals_css__WEBPACK_IMPORTED_MODULE_3__);\n/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! react-redux */ \"react-redux\");\n/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_4__);\n/* harmony import */ var _app_store__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @/app/store */ \"./src/app/store.ts\");\nvar __webpack_async_dependencies__ = __webpack_handle_async_dependencies__([react_toastify__WEBPACK_IMPORTED_MODULE_2__]);\nreact_toastify__WEBPACK_IMPORTED_MODULE_2__ = (__webpack_async_dependencies__.then ? (await __webpack_async_dependencies__)() : __webpack_async_dependencies__)[0];\n\n\n\n\n\n\n\nfunction MyApp({ Component, pageProps }) {\n    return /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(react_redux__WEBPACK_IMPORTED_MODULE_4__.Provider, {\n        store: _app_store__WEBPACK_IMPORTED_MODULE_5__.store,\n        children: /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(_widgets_Layout__WEBPACK_IMPORTED_MODULE_1__.Layout, {\n            children: /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(_barrel_optimize_names_ThemeProvider_mui_material__WEBPACK_IMPORTED_MODULE_6__[\"default\"], {\n                theme: {},\n                children: [\n                    /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(react_toastify__WEBPACK_IMPORTED_MODULE_2__.ToastContainer, {}, void 0, false, {\n                        fileName: \"C:\\\\Users\\\\arist\\\\Documents\\\\source\\\\termodom--ecosystem\\\\src\\\\TD.Quizz\\\\TD.Quiz.Fe\\\\src\\\\pages\\\\_app.tsx\",\n                        lineNumber: 14,\n                        columnNumber: 21\n                    }, this),\n                    /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(Component, {\n                        ...pageProps\n                    }, void 0, false, {\n                        fileName: \"C:\\\\Users\\\\arist\\\\Documents\\\\source\\\\termodom--ecosystem\\\\src\\\\TD.Quizz\\\\TD.Quiz.Fe\\\\src\\\\pages\\\\_app.tsx\",\n                        lineNumber: 15,\n                        columnNumber: 21\n                    }, this)\n                ]\n            }, void 0, true, {\n                fileName: \"C:\\\\Users\\\\arist\\\\Documents\\\\source\\\\termodom--ecosystem\\\\src\\\\TD.Quizz\\\\TD.Quiz.Fe\\\\src\\\\pages\\\\_app.tsx\",\n                lineNumber: 13,\n                columnNumber: 17\n            }, this)\n        }, void 0, false, {\n            fileName: \"C:\\\\Users\\\\arist\\\\Documents\\\\source\\\\termodom--ecosystem\\\\src\\\\TD.Quizz\\\\TD.Quiz.Fe\\\\src\\\\pages\\\\_app.tsx\",\n            lineNumber: 12,\n            columnNumber: 13\n        }, this)\n    }, void 0, false, {\n        fileName: \"C:\\\\Users\\\\arist\\\\Documents\\\\source\\\\termodom--ecosystem\\\\src\\\\TD.Quizz\\\\TD.Quiz.Fe\\\\src\\\\pages\\\\_app.tsx\",\n        lineNumber: 11,\n        columnNumber: 9\n    }, this);\n}\n\n__webpack_async_result__();\n} catch(e) { __webpack_async_result__(e); } });//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9zcmMvcGFnZXMvX2FwcC50c3giLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7Ozs7Ozs7O0FBQTBDO0FBQ0k7QUFFRTtBQUNuQjtBQUNVO0FBQ0g7QUFFckIsU0FBU0ssTUFBTSxFQUFFQyxTQUFTLEVBQUVDLFNBQVMsRUFBWTtJQUM1RCxxQkFDSSw4REFBQ0osaURBQVFBO1FBQUNDLE9BQU9BLDZDQUFLQTtrQkFDbEIsNEVBQUNKLG1EQUFNQTtzQkFDSCw0RUFBQ0MseUZBQWFBO2dCQUFDTyxPQUFPLENBQUM7O2tDQUNuQiw4REFBQ04sMERBQWNBOzs7OztrQ0FDZiw4REFBQ0k7d0JBQVcsR0FBR0MsU0FBUzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7OztBQUs1QyIsInNvdXJjZXMiOlsid2VicGFjazovL3RkLXF1aXp6LWZlLy4vc3JjL3BhZ2VzL19hcHAudHN4P2Y5ZDYiXSwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgTGF5b3V0IH0gZnJvbSBcIkAvd2lkZ2V0cy9MYXlvdXRcIjtcclxuaW1wb3J0IHsgVGhlbWVQcm92aWRlciB9IGZyb20gXCJAbXVpL21hdGVyaWFsXCI7XHJcbmltcG9ydCB7IEFwcFByb3BzIH0gZnJvbSBcIm5leHQvYXBwXCI7XHJcbmltcG9ydCB7IFRvYXN0Q29udGFpbmVyIH0gZnJvbSBcInJlYWN0LXRvYXN0aWZ5XCI7XHJcbmltcG9ydCAnLi8uLi9hcHAvZ2xvYmFscy5jc3MnXHJcbmltcG9ydCB7IFByb3ZpZGVyIH0gZnJvbSBcInJlYWN0LXJlZHV4XCI7XHJcbmltcG9ydCB7IHN0b3JlIH0gZnJvbSBcIkAvYXBwL3N0b3JlXCI7XHJcblxyXG5leHBvcnQgZGVmYXVsdCBmdW5jdGlvbiBNeUFwcCh7IENvbXBvbmVudCwgcGFnZVByb3BzIH06IEFwcFByb3BzKSB7XHJcbiAgICByZXR1cm4gKFxyXG4gICAgICAgIDxQcm92aWRlciBzdG9yZT17c3RvcmV9PlxyXG4gICAgICAgICAgICA8TGF5b3V0PlxyXG4gICAgICAgICAgICAgICAgPFRoZW1lUHJvdmlkZXIgdGhlbWU9e3t9fT5cclxuICAgICAgICAgICAgICAgICAgICA8VG9hc3RDb250YWluZXIgLz5cclxuICAgICAgICAgICAgICAgICAgICA8Q29tcG9uZW50IHsuLi5wYWdlUHJvcHN9IC8+XHJcbiAgICAgICAgICAgICAgICA8L1RoZW1lUHJvdmlkZXI+XHJcbiAgICAgICAgICAgIDwvTGF5b3V0PlxyXG4gICAgICAgIDwvUHJvdmlkZXI+XHJcbiAgICApXHJcbn0iXSwibmFtZXMiOlsiTGF5b3V0IiwiVGhlbWVQcm92aWRlciIsIlRvYXN0Q29udGFpbmVyIiwiUHJvdmlkZXIiLCJzdG9yZSIsIk15QXBwIiwiQ29tcG9uZW50IiwicGFnZVByb3BzIiwidGhlbWUiXSwic291cmNlUm9vdCI6IiJ9\n//# sourceURL=webpack-internal:///./src/pages/_app.tsx\n");

/***/ }),

/***/ "./src/widgets/Layout/index.ts":
/*!*************************************!*\
  !*** ./src/widgets/Layout/index.ts ***!
  \*************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   Layout: () => (/* reexport safe */ _ui_Layout__WEBPACK_IMPORTED_MODULE_0__.Layout)\n/* harmony export */ });\n/* harmony import */ var _ui_Layout__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./ui/Layout */ \"./src/widgets/Layout/ui/Layout.tsx\");\n\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9zcmMvd2lkZ2V0cy9MYXlvdXQvaW5kZXgudHMiLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBb0MiLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly90ZC1xdWl6ei1mZS8uL3NyYy93aWRnZXRzL0xheW91dC9pbmRleC50cz8yYTlhIl0sInNvdXJjZXNDb250ZW50IjpbImV4cG9ydCB7IExheW91dCB9IGZyb20gJy4vdWkvTGF5b3V0JyJdLCJuYW1lcyI6WyJMYXlvdXQiXSwic291cmNlUm9vdCI6IiJ9\n//# sourceURL=webpack-internal:///./src/widgets/Layout/index.ts\n");

/***/ }),

/***/ "./src/widgets/Layout/ui/Layout.tsx":
/*!******************************************!*\
  !*** ./src/widgets/Layout/ui/Layout.tsx ***!
  \******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   Layout: () => (/* binding */ Layout)\n/* harmony export */ });\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react/jsx-dev-runtime */ \"react/jsx-dev-runtime\");\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__);\n\nconst Layout = (props)=>{\n    const { children } = props;\n    return /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n        children: /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"main\", {\n            children: children\n        }, void 0, false, {\n            fileName: \"C:\\\\Users\\\\arist\\\\Documents\\\\source\\\\termodom--ecosystem\\\\src\\\\TD.Quizz\\\\TD.Quiz.Fe\\\\src\\\\widgets\\\\Layout\\\\ui\\\\Layout.tsx\",\n            lineNumber: 12,\n            columnNumber: 13\n        }, undefined)\n    }, void 0, false, {\n        fileName: \"C:\\\\Users\\\\arist\\\\Documents\\\\source\\\\termodom--ecosystem\\\\src\\\\TD.Quizz\\\\TD.Quiz.Fe\\\\src\\\\widgets\\\\Layout\\\\ui\\\\Layout.tsx\",\n        lineNumber: 11,\n        columnNumber: 9\n    }, undefined);\n};\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiLi9zcmMvd2lkZ2V0cy9MYXlvdXQvdWkvTGF5b3V0LnRzeCIsIm1hcHBpbmdzIjoiOzs7Ozs7O0FBTU8sTUFBTUEsU0FBUyxDQUFDQztJQUNuQixNQUFNLEVBQUVDLFFBQVEsRUFBRSxHQUFHRDtJQUVyQixxQkFDSSw4REFBQ0U7a0JBQ0csNEVBQUNDO3NCQUFPRjs7Ozs7Ozs7Ozs7QUFHcEIsRUFBQyIsInNvdXJjZXMiOlsid2VicGFjazovL3RkLXF1aXp6LWZlLy4vc3JjL3dpZGdldHMvTGF5b3V0L3VpL0xheW91dC50c3g/MjRmNSJdLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBSZWFjdE5vZGUgfSBmcm9tIFwicmVhY3RcIjtcclxuXHJcbmludGVyZmFjZSBJTGF5b3V0UHJvcHMge1xyXG4gICAgY2hpbGRyZW46IFJlYWN0Tm9kZVxyXG59XHJcblxyXG5leHBvcnQgY29uc3QgTGF5b3V0ID0gKHByb3BzOiBJTGF5b3V0UHJvcHMpOiBKU1guRWxlbWVudCA9PiB7XHJcbiAgICBjb25zdCB7IGNoaWxkcmVuIH0gPSBwcm9wc1xyXG5cclxuICAgIHJldHVybiAoXHJcbiAgICAgICAgPGRpdj5cclxuICAgICAgICAgICAgPG1haW4+eyBjaGlsZHJlbiB9PC9tYWluPlxyXG4gICAgICAgIDwvZGl2PlxyXG4gICAgKVxyXG59Il0sIm5hbWVzIjpbIkxheW91dCIsInByb3BzIiwiY2hpbGRyZW4iLCJkaXYiLCJtYWluIl0sInNvdXJjZVJvb3QiOiIifQ==\n//# sourceURL=webpack-internal:///./src/widgets/Layout/ui/Layout.tsx\n");

/***/ }),

/***/ "./src/app/globals.css":
/*!*****************************!*\
  !*** ./src/app/globals.css ***!
  \*****************************/
/***/ (() => {



/***/ }),

/***/ "@mui/system":
/*!******************************!*\
  !*** external "@mui/system" ***!
  \******************************/
/***/ ((module) => {

"use strict";
module.exports = require("@mui/system");

/***/ }),

/***/ "@reduxjs/toolkit":
/*!***********************************!*\
  !*** external "@reduxjs/toolkit" ***!
  \***********************************/
/***/ ((module) => {

"use strict";
module.exports = require("@reduxjs/toolkit");

/***/ }),

/***/ "prop-types":
/*!*****************************!*\
  !*** external "prop-types" ***!
  \*****************************/
/***/ ((module) => {

"use strict";
module.exports = require("prop-types");

/***/ }),

/***/ "react":
/*!************************!*\
  !*** external "react" ***!
  \************************/
/***/ ((module) => {

"use strict";
module.exports = require("react");

/***/ }),

/***/ "react-redux":
/*!******************************!*\
  !*** external "react-redux" ***!
  \******************************/
/***/ ((module) => {

"use strict";
module.exports = require("react-redux");

/***/ }),

/***/ "react/jsx-dev-runtime":
/*!****************************************!*\
  !*** external "react/jsx-dev-runtime" ***!
  \****************************************/
/***/ ((module) => {

"use strict";
module.exports = require("react/jsx-dev-runtime");

/***/ }),

/***/ "react/jsx-runtime":
/*!************************************!*\
  !*** external "react/jsx-runtime" ***!
  \************************************/
/***/ ((module) => {

"use strict";
module.exports = require("react/jsx-runtime");

/***/ }),

/***/ "react-toastify":
/*!*********************************!*\
  !*** external "react-toastify" ***!
  \*********************************/
/***/ ((module) => {

"use strict";
module.exports = import("react-toastify");;

/***/ })

};
;

// load runtime
var __webpack_require__ = require("../webpack-runtime.js");
__webpack_require__.C(exports);
var __webpack_exec__ = (moduleId) => (__webpack_require__(__webpack_require__.s = moduleId))
var __webpack_exports__ = __webpack_require__.X(0, ["vendor-chunks/@mui","vendor-chunks/@babel"], () => (__webpack_exec__("./src/pages/_app.tsx")));
module.exports = __webpack_exports__;

})();