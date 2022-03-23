mergeInto (LibraryManager.library,
{
  GetUserData : function(callback_name_in)
  {
    getUserData(UTF8ToString(callback_name_in));
  },

  GetEmail : function(callback_name_in)
  {
    getEmail(UTF8ToString(callback_name_in));
  }

});
