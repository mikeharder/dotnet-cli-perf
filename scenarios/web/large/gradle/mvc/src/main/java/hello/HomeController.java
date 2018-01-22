package hello;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

@Controller
public class HomeController {

    @RequestMapping("/")
    public String index(Model model) {
        model.addAttribute("title", "InitialValue" + " " + ClassLib127.Class001.property() + " " + ClassLib055.Class001.property() + " " + ClassLib041.Class001.property());
        return "index";
    }

}
