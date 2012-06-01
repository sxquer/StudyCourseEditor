using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{
    public class AccountController : Controller
    {
        #region Auto Generated
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName,
                                                      model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 &&
                        returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") &&
                        !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("",
                                             "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password,
                                      model.Email, null, null, true, null,
                                      out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false
                        /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser =
                        Membership.GetUser(User.Identity.Name, true
                            /* userIsOnline */);
                    changePasswordSucceeded =
                        currentUser.ChangePassword(model.OldPassword,
                                                   model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("",
                                             "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #endregion

        /// <summary>
        /// Returns user's unique guid
        /// </summary>
        /// <returns></returns>
        public static Guid? GetUserGuid()
        {
            var user = Membership.GetUser();
            if (user == null || user.ProviderUserKey == null) return null;
            return (Guid)user.ProviderUserKey;
        }

        #region User Permissions
        /// <summary>
        /// Access denied page
        /// </summary>
        /// <param name="message">Cause of rejection</param>
        /// <returns></returns>
        public ActionResult AccessDenied(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        /// <summary>
        /// Check if current user is admin
        /// </summary>
        /// <returns></returns>
        public static bool IsUserAdmin()
        {
            return Roles.GetRolesForUser().Contains("administrator");
        }


        /// <summary>
        /// Check if user is course author
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public static bool IsUserAuthor(Course course)
        {
            return course.UserId != null && course.UserId == GetUserGuid();
        }

        /// <summary>
        /// Check if user is definition author
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        public static bool IsUserAuthor(Definition definition)
        {
            return definition.UserId != null && definition.UserId == GetUserGuid();
        }

        /// <summary>
        /// Check if user has edit rights for course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public static bool CanUserAccess(Course course)
        {
            return IsUserAdmin() || IsUserAuthor(course);
        }

        /// <summary>
        /// Check if user has edit rights for definition
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        public static bool CanUserAccess(Definition definition)
        {
            return IsUserAdmin() || IsUserAuthor(definition);
        }
        #endregion

        #region Status Codes

        private static string ErrorCodeToString(
            MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Имя пользователя занято. Выбирите другое.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "Пользователь с таким email адресом уже существует. Пожалуйста, выбирите другой.";

                case MembershipCreateStatus.InvalidPassword:
                    return
                        "Введен некорректный пароль. Попробуйте другой.";

                case MembershipCreateStatus.InvalidEmail:
                    return
                        "Введен некорретный email адрес. Попробуйте другой.";

                case MembershipCreateStatus.InvalidAnswer:
                    return
                        "Ответ задан не верно. Проверьте значение и попробуйте снова.";

                case MembershipCreateStatus.InvalidQuestion:
                    return
                        "Вопрос задан не верно. Проверьте значение и попробуйте снова.";

                case MembershipCreateStatus.InvalidUserName:
                    return
                        "Недопустимое имя пользователя. Проверьте значение и попробуйте снова.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "Произошла ошибки при попытке связаться с системой авторизации. Попробуйте позже. Если ошибка повторится, обратитесь к администратору.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "Запрос за создание пользователя был отменен. Попробуйте позже. Если проблема повторится, обратитесь к администратору.";

                default:
                    return
                        "Произошла неизвестная ошибка. Попробуйте позже. Если проблема повторится, обратитесь к администратору..";
            }
        }

        #endregion
        
    }
}