package ClassLib042;

public class Class001 {
    public static String property() {
        return "ClassLib042" + " " + ClassLib007.Class001.property() + " " + ClassLib022.Class001.property() + " " + ClassLib012.Class001.property() + " " + ClassLib028.Class001.property();
    }
}
