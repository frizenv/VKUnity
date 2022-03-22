mergeInto (LibraryManager.library,
{
  Auth : function(callback_name_in)
  {
    auth(UTF8ToString(callback_name_in));
  },

  GetUserData : function(callback_name_in)
  {
    getUserData(UTF8ToString(callback_name_in));
  }

});
